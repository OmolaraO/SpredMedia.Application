﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.Utility
{
    internal sealed class SecurityToken
    {
        private readonly byte[] _data;

        public SecurityToken(byte[] data)
        {
            _data = (byte[])data.Clone();
        }

        internal byte[] GetDataNoClone()
        {
            return _data;
        }
    }

    internal class Rfc6238AuthenticationProvider
    {
        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan _timestep = TimeSpan.FromMinutes(3);
        private static readonly Encoding _encoding = new UTF8Encoding(false, true);

        private static int ComputeToOTP(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier, int numberOfDigits = 6)
        {
            // # of 0's = length of pin
            //const int mod = 1000000;
            var mod = (int)Math.Pow(10, numberOfDigits);

            // See https://tools.ietf.org/html/rfc4226
            // We can add an optional modifier
            var timestepAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
            var hash = hashAlgorithm.ComputeHash(ApplyModifier(timestepAsBytes, modifier));

            // Generate DT string
            var offset = hash[hash.Length - 1] & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            var binaryCode = (hash[offset] & 0x7f) << 24
                             | (hash[offset + 1] & 0xff) << 16
                             | (hash[offset + 2] & 0xff) << 8
                             | (hash[offset + 3] & 0xff);

            var code = binaryCode % mod;
            return code;
        }

        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (String.IsNullOrEmpty(modifier))
            {
                return input;
            }

            var modifierBytes = _encoding.GetBytes(modifier);
            var combined = new byte[checked(input.Length + modifierBytes.Length)];
            Buffer.BlockCopy(input, 0, combined, 0, input.Length);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);
            return combined;
        }

        // More info: https://tools.ietf.org/html/rfc6238#section-4
        private static ulong GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - _unixEpoch;
            return (ulong)(delta.Ticks / _timestep.Ticks);
        }

        public static int GenerateCode(SecurityToken securityToken, string modifier = "", int numberOfDigits = 6)
        {

            if (securityToken == null)
            {
                throw new ArgumentNullException("SecurityToken");
            }

            // Allow a variance of no greater than 90 seconds in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken.GetDataNoClone()))
            {
                var code = ComputeToOTP(hashAlgorithm, currentTimeStep, modifier, numberOfDigits);
                return code;
            }
        }

        public static bool ValidateCode(SecurityToken securityToken, int code, string modifier = "", int numberOfDigits = 6)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException("SecurityToken");
            }

            // Allow a variance of no greater than 90 seconds in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken.GetDataNoClone()))
            {
                for (var i = -2; i <= 2; i++)
                {
                    var computedTotp = ComputeToOTP(hashAlgorithm, (ulong)((long)currentTimeStep + i), modifier, numberOfDigits);
                    if (computedTotp == code)
                    {
                        return true;
                    }
                }
            }

            // No match
            return false;
        }
    }
}
