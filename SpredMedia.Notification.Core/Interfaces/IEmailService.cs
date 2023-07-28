using System;
using SpredMedia.CommonLibrary;
using SpredMedia.Notification.Core.DTOs;
using SpredMedia.Notification.Core.Utilities;

namespace SpredMedia.Notification.Core.Interfaces
{
	public interface IEmailService
	{
        Task<ResponseDto<bool>> SendSingleEmail(SingleEmailDTO dto);
        Task<ResponseDto<bool>> SendSingleEmailAtAddress(SingleEmailDTO dto);
        Task<ResponseDto<bool>> SendBulkEmailToSpecific(BulkEmailDTO dto);
        Task<ResponseDto<bool>> SendBulkEmailToAll(BulkEmailToAllDTO dto);
    }
}

