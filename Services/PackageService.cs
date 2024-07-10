using System;
using NBSSR.Network;
using NBSSRServer.Extensions;
using NBSSRServer.MiniDatabase;

namespace NBSSRServer.Services
{
    public class CreatePackageService : NBServiceBase<CreatePackageRequest, CreatePackageResponse>
    {
        public override CreatePackageResponse ProcessMessage(CreatePackageRequest request)
        {
            return CreatePackage(request);
        }

        private CreatePackageResponse CreatePackage(CreatePackageRequest request)
        {
            CreatePackageResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;

            PackageInfo packageInfo = request.packageInfo;
            if (packageInfo == null)
            {
                response.ErrorMsg = "empty param package info.";
                return response;
            }

            if (PackageService.GetPackageInfo(packageInfo.seatType, packageInfo.packageType) != null)
            {
                response.ErrorMsg = "package already exist.";
                return response;
            }

            MiniDataManager.Instance.packageInfoDB.Add(packageInfo);

            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"create package success: {packageInfo.Json()}");

            return response;
        }
    }

    public class UpdatePackageService : NBServiceBase<UpdatePackageRequest, UpdatePackageResponse>
    {
        public override UpdatePackageResponse ProcessMessage(UpdatePackageRequest request)
        {
            return UpdatePackage(request);
        }

        private UpdatePackageResponse UpdatePackage(UpdatePackageRequest request)
        {
            UpdatePackageResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;

            PackageInfo packageInfo = request.packageInfo;
            if (packageInfo == null)
            {
                response.ErrorMsg = "empty param package info.";
                return response;
            }

            if (PackageService.GetPackageInfo(packageInfo.seatType, packageInfo.packageType) == null)
            {
                response.ErrorMsg = "package not exist.";
                return response;
            }

            MiniDataManager.Instance.packageInfoDB.Update((item) =>
            {
                return item.seatType == packageInfo.seatType && item.packageType == packageInfo.packageType;
            }, packageInfo);

            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"update package success: {packageInfo.Json()}");

            return response;
        }
    }

    public class DeletePackageService : NBServiceBase<DeletePackageRequest, DeletePackageResponse>
    {
        public override DeletePackageResponse ProcessMessage(DeletePackageRequest request)
        {
            return DeletePackage(request);
        }

        private DeletePackageResponse DeletePackage(DeletePackageRequest request)
        {
            DeletePackageResponse response = new();
            response.ActionCode = NetMessageActionCode.Failed;

            PackageInfo packageInfo = request.packageInfo;
            if (packageInfo == null)
            {
                response.ErrorMsg = "empty param package info.";
                return response;
            }

            if (PackageService.GetPackageInfo(packageInfo.seatType, packageInfo.packageType) == null)
            {
                response.ErrorMsg = "package not exist.";
                return response;
            }

            if (!MiniDataManager.Instance.packageInfoDB.Remove((item) =>
            {
                return item.seatType == packageInfo.seatType && item.packageType == packageInfo.packageType;
            }))
            {
                response.ErrorMsg = $"delete package failed";
                return response;
            }

            response.ActionCode = NetMessageActionCode.Success;
            logger.LogInfo($"delete package success: {packageInfo.Json()}");

            return response;
        }
    }

    public class PackageService : NBService
    {
        public static PackageInfo GetPackageInfo(SeatType seatType, PackageType packageType)
        {
            return MiniDataManager.Instance.packageInfoDB.Get((item) =>
            {
                return item.seatType == seatType && item.packageType == packageType;
            });
        }
	}
}

