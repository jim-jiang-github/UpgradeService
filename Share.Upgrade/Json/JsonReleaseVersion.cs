using System;
using System.Linq;

namespace Share.Upgrade.Json
{
    /// <summary>
    /// 发布版本
    /// </summary>
    public class JsonReleaseVersion
    {
        /// <summary> 发布类型
        /// </summary>
        public enum ReleaseType
        {
            /// <summary>
            /// 选择更新
            /// </summary>
            Choice = 0,
            /// <summary>
            /// 强制更新
            /// </summary>
            Force = 1
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 发布的文件详细信息
        /// </summary>
        public JsonFileDetail[] Files { get; set; }
        /// <summary>
        /// 需要删除的文件
        /// </summary>
        public string[] Deletes { get; set; }
        /// <summary>
        /// 更新内容
        /// </summary>
        public string UpdateContent { get; set; }
        /// <summary>
        /// 发布类型
        /// </summary>
        public ReleaseType Type { get; set; }

        public static JsonReleaseVersion operator +(JsonReleaseVersion jsonReleaseVersion1, JsonReleaseVersion jsonReleaseVersion2)
        {
            JsonReleaseVersion highVersion = new Version(jsonReleaseVersion1.Version) > new Version(jsonReleaseVersion2.Version) ? jsonReleaseVersion1 : jsonReleaseVersion2;
            JsonReleaseVersion lowVersion = new Version(jsonReleaseVersion1.Version) > new Version(jsonReleaseVersion2.Version) ? jsonReleaseVersion2 : jsonReleaseVersion1;
            ReleaseType releaseType = (highVersion.Type == ReleaseType.Force || lowVersion.Type == ReleaseType.Force) ? ReleaseType.Force : ReleaseType.Choice;
            return new JsonReleaseVersion()
            {
                Version = highVersion.Version,
                UpdateContent = highVersion.UpdateContent,
                Type = (highVersion.Type == ReleaseType.Force || lowVersion.Type == ReleaseType.Force) ? ReleaseType.Force : ReleaseType.Choice,
                Deletes = lowVersion.Deletes
                .Where(d => highVersion.Files.FirstOrDefault(f => f.Name == d) == null)
                .Concat(highVersion.Deletes)
                .ToArray(),
                Files = lowVersion.Files
                .Where(f => !highVersion.Deletes.Contains(f.Name))
                .Select(f => highVersion.Files.FirstOrDefault(fd => fd.Name == f.Name) ?? f)
                .Concat(highVersion.Files.Where(f => lowVersion.Files.FirstOrDefault(fd => fd.Name == f.Name) == null))
                .ToArray()
            };
        }
    }
}
