/**
The MIT License (MIT)

Copyright (c) 2016 Marek GAMELASTER Kraus

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Concept2
{

    public class PMUSBInterface
    {
        public class CSAFECommand
        {
            public CSAFECommand(ushort DeviceNumber)
            {
                this.DeviceNumber = DeviceNumber;
                this.CommandsBytes = new List<uint>();
            }

            public ushort DeviceNumber { get; set; }
            public List<uint> CommandsBytes { get; set; }



            public uint[] Execute()
            {
                ushort datasize = 64;
                uint[] data = new uint[64];
                short error = tkcmdsetCSAFE_command(DeviceNumber, Convert.ToUInt16(CommandsBytes.Count), CommandsBytes.ToArray(), ref datasize, data);
                if (error != 0)
                {
                    string message = getErrorText(error);
                    throw new PMUSBException("Cannot execute command. Error: " + message, error);
                }
                return data;
            }
        }

        public class PMUSBException : Exception
        {
            public PMUSBException(string message, short ErrorCode) : base(message + ErrorCode.ToString())
            {
                this.ErrorCode = ErrorCode;
                StringBuilder nameBuffer = new StringBuilder(new string(' ', 40));
                tkcmdsetDDI_get_error_name(ErrorCode, nameBuffer, 40);
                this.ErrorName = nameBuffer.ToString();
                StringBuilder textBuffer = new StringBuilder(new string(' ', 200));
                tkcmdsetDDI_get_error_text(ErrorCode, textBuffer, 200);
                this.ErrorDescription = textBuffer.ToString();
            }

            public short ErrorCode { get; }
            public string ErrorName { get; }
            public string ErrorDescription { get; }
        }

        public static void Initialize()
        {
            /*try
            {*/
            short error = tkcmdsetDDI_init();
            if (error != 0)
            {
                throw new PMUSBException("Cannot initialize a USB connection!", error);
            }
            /*}
            catch(Exception ex)
            {
                throw new Exception("Cannot initialize a USB connection!", ex);
            }*/
        }

        public static void InitializeProtocol(ushort timeout)
        {
            //try
            //{
            short error = tkcmdsetCSAFE_init_protocol(timeout);
            if (error != 0)
            {
                throw new PMUSBException("Cannot initialize a CSAFE protocol!", error);
            }
            /*}
            catch (Exception ex)
            {
                throw new Exception("Cannot initialize a CSAFE protocol!", ex);
            }*/
        }

        public static ushort[] findDevices() {
            byte devNum = 0;
            ushort[] portlist = new ushort[1000];
            tkcmdsetDDI_find_devices(PMtype.PM5_PRODUCT_NAME.ToString(), ref devNum, portlist);
            return portlist;
        }

        public static string getErrorText(short errCode) {
            StringBuilder errText = new StringBuilder();
            tkcmdsetDDI_get_error_text(errCode, errText, 10000);
            return errText.ToString();
        }

        public static string getSerialNumber(ushort portnum) {
            var str = new StringBuilder();
            tkcmdsetDDI_serial_number(portnum, str, 64);
            return str.ToString();
        }

        //little tweak to developers
        public static ushort DiscoverPMs(PMtype PMType)
        {
            string PMname = "";
            switch (PMType)
            {
                case PMtype.PM3_PRODUCT_NAME: PMname = "Concept II PM3"; break;
                case PMtype.PM3_PRODUCT_NAME2: PMname = "Concept2 Performance Monitor 3 (PM3)"; break;
                case PMtype.PM3TESTER_PRODUCT_NAME: PMname = "Concept 2 PM3 Tester"; break;
                case PMtype.PM4_PRODUCT_NAME: PMname = "Concept2 Performance Monitor 4 (PM4)"; break;
                case PMtype.PM5_PRODUCT_NAME: PMname = "Concept2 Performance Monitor 5 (PM5)"; break;
            }
            //try
            //{
            ushort count = 0;
            short error = tkcmdsetDDI_discover_pm3s(PMname, 0, ref count);
            if (error != 0)
            {
                throw new PMUSBException("Cannot initialize a CSAFE protocol!", error);
            }
            return count;
            /*}
            catch (Exception ex)
            {
                throw new Exception("Cannot initialize a CSAFE protocol!", ex);
            }*/
        }

        public static uint getMSB(uint[] bytes, uint byteIdx)
        {
            uint ret = 0; 
            if (bytes.Length >= 3)
            {
                ret = bytes[byteIdx];
            }
            return ret;
        }

        public static string BytesToCMDstring(uint[] bytes)
        {
            string str = "";
            foreach (uint byt in bytes)
            {
                str += byt.ToString() + " ";
            }
            return str;
        }

        public static void ShutdownAll()
        {
            short error = tkcmdsetDDI_shutdown_all();
            if (error != 0) throw new PMUSBException("Cannot shutdown all connections!", error);
        }


        public enum PMtype
        {
            PM3_PRODUCT_NAME,
            PM3_PRODUCT_NAME2,
            PM3TESTER_PRODUCT_NAME,
            PM4_PRODUCT_NAME,
            PM5_PRODUCT_NAME,
        }

        public enum CSAFEUnits : uint
        {
            Meters = 0x24,
            Kilometers = 0x21
        }

        public enum WorkoutTypes : uint
        {
            Programmed = 0x00,
            TwoKm500Split = 0x01,
            FiveKm1000Split = 0x02,
            TenKm2000Split = 0x03,
            ThirdyMin6minSplit = 0x04,
            FiftyMetersIntMinuteRest = 0x05
        }

        public enum CSAFECommands : uint
        {
            CSAFE_GETSTATUS_CMD = 0x80,
            CSAFE_RESET_CMD = 0x81,
            CSAFE_GOIDLE_CMD = 0x82,
            CSAFE_GOHAVEID_CMD = 0x83,
            CSAFE_GOINUSE_CMD = 0x85,
            CSAFE_GOFINISHED_CMD = 0x86,
            CSAFE_GOREADY_CMD = 0x87,
            CSAFE_BADID_CMD = 0x88,
            CSAFE_GETVERSION_CMD = 0x91,
            CSAFE_GETID_CMD = 0x92,
            CSAFE_GETUNITS_CMD = 0x93,
            CSAFE_GETSERIAL_CMD = 0x94,
            CSAFE_GETLIST_CMD = 0x98,
            CSAFE_GETUTILIZATION_CMD = 0x99,
            CSAFE_GETMOTORCURRENT_CMD = 0x9A,
            CSAFE_GETODOMETER_CMD = 0x9B,
            CSAFE_GETERRORCODE_CMD = 0x9C,
            CSAFE_GETSERVICECODE_CMD = 0x9D,
            CSAFE_GETUSERCFG1_CMD = 0x9E,
            CSAFE_GETUSERCFG2_CMD = 0x9F,
            CSAFE_GETTWORK_CMD = 0xA0,
            CSAFE_GETHORIZONTAL_CMD = 0xA1,
            CSAFE_GETVERTICAL_CMD = 0xA2,
            CSAFE_GETCALORIES_CMD = 0xA3,
            CSAFE_GETPROGRAM_CMD = 0xA4,
            CSAFE_GETSPEED_CMD = 0xA5,
            CSAFE_GETPACE_CMD = 0xA6,
            CSAFE_GETCADENCE_CMD = 0xA7,
            CSAFE_GETGRADE_CMD = 0xA8,
            CSAFE_GETGEAR_CMD = 0xA9,
            CSAFE_GETUPLIST_CMD = 0xAA,
            CSAFE_GETUSERINFO_CMD = 0xAB,
            CSAFE_GETTORQUE_CMD = 0xAC,
            CSAFE_GETHRCUR_CMD = 0xB0,
            CSAFE_GETHRTZONE_CMD = 0xB2,
            CSAFE_GETMETS_CMD = 0xB3,
            CSAFE_GETPOWER_CMD = 0xB4,
            CSAFE_GETHRAVG_CMD = 0xB5,
            CSAFE_GETHRMAX_CMD = 0xB6,
            CSAFE_GETUSERDATA1_CMD = 0xBE,
            CSAFE_GETUSERDATA2_CMD = 0xBF,
            CSAFE_SETUSERCFG1_CMD = 0x1A,
            CSAFE_SETTWORK_CMD = 0x20,
            CSAFE_SETHORIZONTAL_CMD = 0x21,
            CSAFE_SETPROGRAM_CMD = 0x24,
            CSAFE_SETTARGETHR_CMD = 0x30,
            CSAFE_PM_GET_WORKDISTANCE = 0xA3,
            CSAFE_PM_GET_WORKTIME = 0xA0,
            CSAFE_PM_GET_WORKOUTSTATE = 0x8D,
            CSAFE_PM_SET_SPLITDURATION = 0x05,
            CSAFE_PM_GET_FORCEPLOTDATA = 0x6B,
            CSAFE_PM_GET_DRAGFACTOR = 0xC1,
            CSAFE_PM_GET_STROKESTATE = 0xBF,
            CSAFE_UNITS_METER = 0x24
        }

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern short tkcmdsetDDI_init();

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void tkcmdsetDDI_get_error_name(
            short ecode,
            StringBuilder nameptr,
            ushort namelen);

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern short tkcmdsetDDI_shutdown_all();

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void tkcmdsetDDI_get_error_text(
            short ecode,
            StringBuilder textptr,
            ushort textlen);

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern short tkcmdsetDDI_discover_pm3s(
           string product_name,
           ushort starting_address,
           ref ushort num_units);

        [DllImport("PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern short tkcmdsetCSAFE_init_protocol(ushort timeout);

        [DllImport("PM3CsafeCP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern short tkcmdsetCSAFE_command(
           ushort unit_address,
           ushort cmd_data_size,
           uint[] cmd_data,
           ref ushort rsp_data_size,
           uint[] rsp_data);

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort tkcmdsetDDI_find_devices(
            [In] string product_name,
            [In, Out] ref byte num_found,
            [In] ushort[] port_list);

        [DllImport("PM3DDICP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort tkcmdsetDDI_serial_number(
            [In] ushort port,
            [Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder ser_ptr,
            [In] byte ser_len);
    }
}