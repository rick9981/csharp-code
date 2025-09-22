using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppBatchOpcUa
{
    // NodeId 类定义（按照OPC UA格式）
    public class JNodeId
    {
        public string FullNodeId { get; set; } = "";
        public int Namespace { get; set; } = 2; // 默认命名空间为2
        public string Identifier { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string ChannelName { get; set; } = "";
        public string TagAddress { get; set; } = "";
        public int DataType { get; set; }
        public int ReadWriteAccess { get; set; }

        // 生成标准的NodeId字符串格式：ns=2;s=Channel.Device.TagName
        public void GenerateFullNodeId()
        {
            FullNodeId = $"ns={Namespace};s={ChannelName}.{DeviceName}.{DisplayName}";
            Identifier = $"{ChannelName}.{DeviceName}.{DisplayName}";
        }

        public override string ToString()
        {
            return FullNodeId;
        }
    }

    // JSON数据模型
    public class LmesRootConfig
    {
        [JsonPropertyName("export_info")]
        public ExportInfo ExportInfo { get; set; } = new ExportInfo();

        [JsonPropertyName("channel_info")]
        public ChannelInfo ChannelInfo { get; set; } = new ChannelInfo();

        [JsonPropertyName("devices")]
        public object Devices { get; set; } // 注意：这是JSON字符串
    }

    public class ExportInfo
    {
        [JsonPropertyName("channel_name")]
        public string ChannelName { get; set; } = "";

        [JsonPropertyName("export_time")]
        public string ExportTime { get; set; } = "";

        [JsonPropertyName("total_devices")]
        public int TotalDevices { get; set; }

        [JsonPropertyName("total_tags")]
        public int TotalTags { get; set; }
    }

    public class ChannelInfo
    {
        [JsonPropertyName("PROJECT_ID")]
        public long ProjectId { get; set; }

        [JsonPropertyName("common.ALLTYPES_NAME")]
        public string Name { get; set; } = "";

        [JsonPropertyName("common.ALLTYPES_DESCRIPTION")]
        public string Description { get; set; } = "";
    }

    public class DeviceContainer
    {
        [JsonPropertyName("device_name")]
        public string DeviceName { get; set; } = "";

        [JsonPropertyName("device_info")]
        public DeviceInfo DeviceInfo { get; set; } = new DeviceInfo();

        [JsonPropertyName("tags")]
        public List<TagInfo> Tags { get; set; } = new List<TagInfo> ();
    }

    public class DeviceInfo
    {
        [JsonPropertyName("common.ALLTYPES_NAME")]
        public string Name { get; set; } = "";
    }

    public class TagInfo
    {
        [JsonPropertyName("common.ALLTYPES_NAME")]
        public string Name { get; set; } = "";

        [JsonPropertyName("common.ALLTYPES_DESCRIPTION")]
        public string Description { get; set; } = "";

        [JsonPropertyName("servermain.TAG_ADDRESS")]
        public string Address { get; set; } = "";

        [JsonPropertyName("servermain.TAG_DATA_TYPE")]
        public int DataType { get; set; }

        [JsonPropertyName("servermain.TAG_READ_WRITE_ACCESS")]
        public int ReadWriteAccess { get; set; }
    }
}
