using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Models.Response
{
    public class LoginResponse
    {
        [JsonProperty("sysUser")]
        public SysUser SysUser { get; set; }

        [JsonProperty("mainMerchantDevice")]
        public MainMerchantDevice MainMerchantDevice { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedBy")]
        public int ModifiedBy { get; set; }

        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("fcmId")]
        public string FcmId { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("mainMerchantDevices")]
        public List<object> MainMerchantDevices { get; set; }
    }

    public class MainMerchantDevice
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedBy")]
        public int ModifiedBy { get; set; }

        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        [JsonProperty("deviceDisplayName")]
        public string DeviceDisplayName { get; set; }

        [JsonProperty("merchantStoreId")]
        public string MerchantStoreId { get; set; }

        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("device")]
        public Device Device { get; set; }

        [JsonProperty("merchant")]
        public string Merchant { get; set; }

        [JsonProperty("merchantStore")]
        public string MerchantStore { get; set; }

        [JsonProperty("mainMerchantProductDevices")]
        public List<object> MainMerchantProductDevices { get; set; }
    }

    public class SysUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("identityCard")]
        public string IdentityCard { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("sysUserTypeId")]
        public string SysUserTypeId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedBy")]
        public int ModifiedBy { get; set; }

        [JsonProperty("modifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("isChangePassNextLogin")]
        public string IsChangePassNextLogin { get; set; }

        [JsonProperty("isDeleted")]
        public string IsDeleted { get; set; }

        [JsonProperty("sysUserProfileId")]
        public string SysUserProfileId { get; set; }

        [JsonProperty("passwordSalt")]
        public string PasswordSalt { get; set; }
    }
}
