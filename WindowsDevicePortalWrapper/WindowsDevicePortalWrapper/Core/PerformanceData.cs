﻿//----------------------------------------------------------------------------------------------
// <copyright file="PerformanceData.cs" company="Microsoft Corporation">
//     Licensed under the MIT License. See LICENSE.TXT in the project root license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Tools.WindowsDevicePortal
{
    /// <content>
    /// Wrappers for Performance methods
    /// </content>
    public partial class DevicePortal
    {
        /// <summary>
        /// API for getting all running processes
        /// </summary>
        public static readonly string RunningProcessApi = "api/resourcemanager/processes";

        /// <summary>
        /// API for getting system performance
        /// </summary>
        public static readonly string SystemPerfApi = "api/resourcemanager/systemperf";

        /// <summary>
        /// Gets the collection of processes running on the device.
        /// </summary>
        /// <returns>RunningProcesses object containing the list of running processes.</returns>
        public async Task<RunningProcesses> GetRunningProcesses()
        {
            return await this.Get<RunningProcesses>(RunningProcessApi);
        }

        /// <summary>
        /// Gets system performance information for the device.
        /// </summary>
        /// <returns>SystemPerformanceInformation object containing information such as memory usage.</returns>
        public async Task<SystemPerformanceInformation> GetSystemPerf()
        {
            return await this.Get<SystemPerformanceInformation>(SystemPerfApi);
        }

        #region Device contract

        /// <summary>
        /// Process Info
        /// </summary>
        [DataContract]
        public class DeviceProcessInfo
        {
            /// <summary>
            /// Gets or sets the app name
            /// </summary>
            [DataMember(Name = "AppName")]
            public string AppName { get; set; }

            /// <summary>
            /// Gets or sets CPU usage
            /// </summary>
            [DataMember(Name = "CPUUsage")]
            public float CpuUsage { get; set; }

            /// <summary>
            /// Gets or sets the image name
            /// </summary>
            [DataMember(Name = "ImageName")]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the process id
            /// </summary>
            [DataMember(Name = "ProcessId")]
            public int ProcessId { get; set; }

            /// <summary>
            /// Gets or sets the owner name
            /// </summary>
            [DataMember(Name = "UserName")]
            public string UserName { get; set; }

            /// <summary>
            /// Gets or sets the package full name
            /// </summary>
            [DataMember(Name = "PackageFullName")]
            public string PackageFullName { get; set; }

            /// <summary>
            /// Gets or sets the Page file usage info
            /// </summary>
            [DataMember(Name = "PageFileUsage")]
            public uint PageFile { get; set; }

            /// <summary>
            /// Gets or sets the working set size
            /// </summary>
            [DataMember(Name = "WorkingSetSize")]
            public uint WorkingSet { get; set; }

            /// <summary>
            /// String representation of a process
            /// </summary>
            /// <returns>String representation</returns>
            public override string ToString()
            {
                return string.Format("{0} ({1})", this.AppName, this.Name);
            }
        }

        /// <summary>
        /// GPU Adaptors
        /// </summary>
        [DataContract]
        public class GpuAdapters
        {
            /// <summary>
            /// Gets or sets total Dedicated memory
            /// </summary>
            [DataMember(Name = "DedicatedMemory")]
            public uint DedicatedMemory { get; set; }

            /// <summary>
            /// Gets or sets used Dedicated memory
            /// </summary>
            [DataMember(Name = "DedicatedMemoryUsed")]
            public uint DedicatedMemoryUsed { get; set; }

            /// <summary>
            /// Gets or sets description
            /// </summary>
            [DataMember(Name = "Description")]
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets system memory
            /// </summary>
            [DataMember(Name = "SystemMemory")]
            public uint SystemMemory { get; set; }

            /// <summary>
            /// Gets or sets memory used
            /// </summary>
            [DataMember(Name = "SystemMemoryUsed")]
            public uint SystemMemoryUsed { get; set; }

            /// <summary>
            /// Gets or sets engines utilization
            /// </summary>
            [DataMember(Name = "EnginesUtilization")]
            public List<float> EnginesUtilization { get; set; }
        }

        /// <summary>
        /// GPU performance data
        /// </summary>
        [DataContract]
        public class GpuPerformanceData
        {
            /// <summary>
            /// Gets or sets list of available adapters
            /// </summary>
            [DataMember(Name = "AvailableAdapters")]
            public List<GpuAdapters> Adapters { get; set; }
        }

        /// <summary>
        /// Network performance data
        /// </summary>
        [DataContract]
        public class NetworkPerformanceData
        {
            /// <summary>
            /// Gets or sets bytes in
            /// </summary>
            [DataMember(Name = "NetworkInBytes")]
            public int BytesIn { get; set; }

            /// <summary>
            ///  Gets or sets bytes out
            /// </summary>
            [DataMember(Name = "NetworkOutBytes")]
            public int BytesOut { get; set; }
        }

        /// <summary>
        /// Running processes
        /// </summary>
        [DataContract]
        public class RunningProcesses
        {
            /// <summary>
            /// Gets or sets processes info
            /// </summary>
            [DataMember(Name = "Processes")]
            public DeviceProcessInfo[] Processes { get; set; }

            /// <summary>
            /// Checks to see if this process Id is in the list of processes
            /// </summary>
            /// <param name="processId">Process to look for</param>
            /// <returns>whether the process id was found</returns>
            public bool Contains(int processId)
            {
                bool found = false;

                if (this.Processes != null)
                {
                    foreach (DeviceProcessInfo pi in this.Processes)
                    {
                        if (pi.ProcessId == processId)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                return found;
            }

            /// <summary>
            /// Checks for a given package name
            /// </summary>
            /// <param name="packageName">Name of the package to look for</param>
            /// <param name="caseSensitive">Whether we should be case sensitive in our search</param>
            /// <returns>Whether the package was found</returns>
            public bool Contains(string packageName, bool caseSensitive = true)
            {
                bool found = false;

                if (this.Processes != null)
                {
                    foreach (DeviceProcessInfo pi in this.Processes)
                    {
                        if (string.Compare(
                                pi.PackageFullName,
                                packageName,
                                caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                return found;
            }
        }

        /// <summary>
        /// System performance information
        /// </summary>
        [DataContract]
        public class SystemPerformanceInformation
        {
            /// <summary>
            /// Gets or sets available pages
            /// </summary>
            [DataMember(Name = "AvailablePages")]
            public int AvailablePages { get; set; }

            /// <summary>
            /// Gets or sets commit limit
            /// </summary>
            [DataMember(Name = "CommitLimit")]
            public int CommitLimit { get; set; }

            /// <summary>
            /// Gets or sets committed pages
            /// </summary>
            [DataMember(Name = "CommittedPages")]
            public int CommittedPages { get; set; }

            /// <summary>
            /// Gets or sets CPU load
            /// </summary>
            [DataMember(Name = "CpuLoad")]
            public int CpuLoad { get; set; }

            /// <summary>
            /// Gets or sets IO Other Speed
            /// </summary>
            [DataMember(Name = "IOOtherSpeed")]
            public int IoOtherSpeed { get; set; }

            /// <summary>
            /// Gets or sets IO Read speed
            /// </summary>
            [DataMember(Name = "IOReadSpeed")]
            public int IoReadSpeed { get; set; }

            /// <summary>
            /// Gets or sets IO write speed
            /// </summary>
            [DataMember(Name = "IOWriteSpeed")]
            public int IoWriteSpeed { get; set; }

            /// <summary>
            /// Gets or sets Non paged pool pages
            /// </summary>
            [DataMember(Name = "NonPagedPoolPages")]
            public int NonPagedPoolPages { get; set; }

            /// <summary>
            /// Gets or sets page size
            /// </summary>
            [DataMember(Name = "PageSize")]
            public int PageSize { get; set; }

            /// <summary>
            /// Gets or sets paged pool pages
            /// </summary>
            [DataMember(Name = "PagedPoolPages")]
            public int PagedPoolPages { get; set; }

            /// <summary>
            /// Gets or sets total installed in KB
            /// </summary>
            [DataMember(Name = "TotalInstalledInKb")]
            public int TotalInstalledKb { get; set; }

            /// <summary>
            /// Gets or sets total pages
            /// </summary>
            [DataMember(Name = "TotalPages")]
            public int TotalPages { get; set; }

            /// <summary>
            /// Gets or sets GPU data
            /// </summary>
            [DataMember(Name = "GPUData")]
            public GpuPerformanceData GpuData { get; set; }

            /// <summary>
            /// Gets or sets Networking data
            /// </summary>
            [DataMember(Name = "NetworkingData")]
            public NetworkPerformanceData NetworkData { get; set; }
        }

        #endregion // Device contract
    }
}
