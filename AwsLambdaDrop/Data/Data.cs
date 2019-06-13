using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AwsLambdaDrop.Data
{
    public partial class AwsLinkingRequestObject
    {
        [JsonProperty("MasterAccountEmail")]
        public string MasterAccountEmail { get; set; }
        [JsonProperty("CallerIdentity")]
        public CallerIdentity CallerIdentity { get; set; }

        [JsonProperty("SecurityGroup")]
        public SecurityGroup SecurityGroup { get; set; }

        [JsonProperty("SecurityUser")]
        public SecurityUser SecurityUser { get; set; }

        [JsonProperty("AccessKey")]
        public AwsLinkingRequestObjectAccessKey AccessKey { get; set; }
    }

    public partial class AwsLinkingRequestObjectAccessKey
    {
        [JsonProperty("AccessKey")]
        public AccessKeyAccessKey AccessKey { get; set; }
    }

    public partial class AccessKeyAccessKey
    {
        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("AccessKeyId")]
        public string AccessKeyId { get; set; }

        [JsonProperty("CreateDate")]
        public DateTimeOffset CreateDate { get; set; }

        [JsonProperty("SecretAccessKey")]
        public string SecretAccessKey { get; set; }
    }

    public partial class CallerIdentity
    {
        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("Arn")]
        public string Arn { get; set; }
    }

    public partial class SecurityGroup
    {
        [JsonProperty("Group")]
        public Group Group { get; set; }
    }

    public partial class Group
    {
        [JsonProperty("Arn")]
        public string Arn { get; set; }

        [JsonProperty("GroupName")]
        public string GroupName { get; set; }

        [JsonProperty("CreateDate")]
        public DateTimeOffset CreateDate { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("GroupId")]
        public string GroupId { get; set; }
    }

    public partial class SecurityUser
    {
        [JsonProperty("User")]
        public User User { get; set; }
    }

    public partial class User
    {
        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("Arn")]
        public string Arn { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("CreateDate")]
        public DateTimeOffset CreateDate { get; set; }
    }

    public partial class AwsLinkingRequestObject
    {
        public static AwsLinkingRequestObject FromJson(string json) => JsonConvert.DeserializeObject<AwsLinkingRequestObject>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AwsLinkingRequestObject self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
