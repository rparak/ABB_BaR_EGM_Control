/* ------------------------------------------------------------------------------------------------------------------------ // 
// ------------------------------------------------------------------------------------------------------------------------ //
// ------------------------------------------------------------------------------------------------------------------------ //
MIT License

Copyright(c) 2020 Roman Parak

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

// ------------------------------------------------------------------------------------------------------------------------ //
// ------------------------------------------------------------------------------------------------------------------------ //
// ------------------------------------------------------------------------------------------------------------------------ //

Author   : Roman Parak
Email    : Roman.Parak @outlook.com
Github   : https://github.com/rparak
File Name: Program.cs

// ------------------------------------------------------------------------------------------------------------------------ //
// ------------------------------------------------------------------------------------------------------------------------ //
// ------------------------------------------------------------------------------------------------------------------------ */

// ------------------------------------------------------------------------------------------------------------------------//
// ----------------------------------------------------- LIBRARIES --------------------------------------------------------//
// ------------------------------------------------------------------------------------------------------------------------//

// -------------------- System -------------------- //
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
// -------------------- ABB EGM -------------------- //
using abb.egm;
// -------------------- OPCUA -------------------- //
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace EGM_OPCUa_ABB_BR_Client
{
    class Program
    {
        // -------------------- ApplicationConfiguration -------------------- //
        static ApplicationConfiguration client_configuration_r = new ApplicationConfiguration();
        static ApplicationConfiguration client_configuration_w = new ApplicationConfiguration();
        // -------------------- EndpointDescription -------------------- //
        static EndpointDescription client_end_point_r, client_end_point_w;
        // -------------------- Session -------------------- //
        static Session client_session_r, client_session_w;
        // -------------------- Thread -------------------- //
        static Thread opcua_client_r_Thread, opcua_client_w_Thread;
        static Thread egm_client_rw;
        // -------------------- NodeId -------------------- //
        static NodeId node_read_opcua_start_j, node_read_opcua_start_c;
        static NodeId node_read_opcua_start_r_j, node_read_opcua_start_r_c;
        static NodeId node_read_opcua_j1, node_read_opcua_j2, node_read_opcua_j3;
        static NodeId node_read_opcua_j4, node_read_opcua_j5, node_read_opcua_j6;
        static NodeId node_read_opcua_c_x, node_read_opcua_c_y, node_read_opcua_c_z;
        static NodeId node_read_opcua_c_rx, node_read_opcua_c_ry, node_read_opcua_c_rz;
        // -------------------- String -------------------- //
        static string program_name;
        static string node_write_opcua_j1, node_write_opcua_j2, node_write_opcua_j3;
        static string node_write_opcua_j4, node_write_opcua_j5, node_write_opcua_j6;
        static string node_write_opcua_c_x, node_write_opcua_c_y, node_write_opcua_c_z;
        static string node_write_opcua_c_rx, node_write_opcua_c_ry, node_write_opcua_c_rz;
        static string node_read_egm_j1, node_read_egm_j2, node_read_egm_j3;
        static string node_read_egm_j4, node_read_egm_j5, node_read_egm_j6;
        static string node_read_egm_c_x, node_read_egm_c_y, node_read_egm_c_z;
        static string node_read_egm_c_rx, node_read_egm_c_ry, node_read_egm_c_rz;
        // -------------------- Bool -------------------- //
        static bool read_start_joint, read_start_cartesian;
        static bool read_start_r_joint, read_start_r_cartesian;
        static bool opcua_c_r_while, opcua_c_w_while;
        static bool egm_c_rw_while;
        // -------------------- Float -------------------- //
        static float read_opcua_j1, read_opcua_j2, read_opcua_j3;
        static float read_opcua_j4, read_opcua_j5, read_opcua_j6;
        static float read_opcua_c_x, read_opcua_c_y, read_opcua_c_z;
        static float read_opcua_c_rx, read_opcua_c_ry, read_opcua_c_rz;
        // -------------------- Uint -------------------- //
        static uint sequence_number = 0;

        // ------------------------------------------------------------------------------------------------------------------------//
        // ------------------------------------------------ MAIN FUNCTION {Cyclic} ------------------------------------------------//
        // ------------------------------------------------------------------------------------------------------------------------//
        static void Main(string[] args)
        {
            // ------------------------ Initialization { Console app. Write } ------------------------//
            Console.WriteLine("[INFO] Externally Guided Motion (EGM)");
            Console.WriteLine("[INFO] OPCUa Communication {B&R Auotmation PLC}");
            Console.WriteLine("[INFO] Author: Roman Parak");
            // ------------------------ Initialization { OPCUa Config.} ------------------------//
            // PLC IP Address
            string ip_adr_plc = "127.0.0.1";
            // PLC Port
            string port_adr_plc = "4840";
            // Write IP, PORT {PLC}
            Console.WriteLine("[INFO] B&R Automation (PLC: IP Address - {0}, Port - {1})", ip_adr_plc, port_adr_plc);

            // ------------------------ Initialization { EGM Config.} ------------------------//
            // Robot Port
            int port_adr_robot = 6511;
            // Write PORT {ABB ROBOT}
            Console.WriteLine("[INFO] ABB Robotics (Industrial Robot: Port - {0})", port_adr_robot);

            // ------------------------ Main Block { Control of the PLC (B&R) } ------------------------//
            try
            {
                // ------------------------ Communication in progress ------------------------//
                Console.WriteLine("[INFO] Communication in progress.");

                // Program name {Task}
                program_name = "Server_t";
                // Node {Read - Start Write data}
                node_read_opcua_start_j = "ns=6;s=::" + program_name + ":abb_control.Command.joint.is_started";
                node_read_opcua_start_c = "ns=6;s=::" + program_name + ":abb_control.Command.cartesian.is_started";
                // Node {Read - Start Read data}
                node_read_opcua_start_r_j = "ns=6;s=::" + program_name + ":abb_control.Command.joint.is_read";
                node_read_opcua_start_r_c = "ns=6;s=::" + program_name + ":abb_control.Command.cartesian.is_read";
                // Node {Read - Joint}
                node_read_opcua_j1 = "ns=6;s=::" + program_name + ":abb_control.Joint.Write.J1";
                node_read_opcua_j2 = "ns=6;s=::" + program_name + ":abb_control.Joint.Write.J2";
                node_read_opcua_j3 = "ns=6;s=::" + program_name + ":abb_control.Joint.Write.J3";
                node_read_opcua_j4 = "ns=6;s=::" + program_name + ":abb_control.Joint.Write.J4";
                node_read_opcua_j5 = "ns=6;s=::" + program_name + ":abb_control.Joint.Write.J5";
                node_read_opcua_j6 = "ns=6;s=::" + program_name + ":abb_control.Joint.Write.J6";
                // Node {Read - Cartesian}
                node_read_opcua_c_x = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Write.X";
                node_read_opcua_c_y = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Write.Y";
                node_read_opcua_c_z = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Write.Z";
                node_read_opcua_c_rx = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Write.RX";
                node_read_opcua_c_ry = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Write.RY";
                node_read_opcua_c_rz = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Write.RZ";
                // Node {Write - Joint}
                node_write_opcua_j1 = "ns=6;s=::" + program_name + ":abb_control.Joint.Read.J1";
                node_write_opcua_j2 = "ns=6;s=::" + program_name + ":abb_control.Joint.Read.J2";
                node_write_opcua_j3 = "ns=6;s=::" + program_name + ":abb_control.Joint.Read.J3";
                node_write_opcua_j4 = "ns=6;s=::" + program_name + ":abb_control.Joint.Read.J4";
                node_write_opcua_j5 = "ns=6;s=::" + program_name + ":abb_control.Joint.Read.J5";
                node_write_opcua_j6 = "ns=6;s=::" + program_name + ":abb_control.Joint.Read.J6";
                // Node {Write - Cartesian}
                node_write_opcua_c_x = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Read.X";
                node_write_opcua_c_y = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Read.Y";
                node_write_opcua_c_z = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Read.Z";
                node_write_opcua_c_rx = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Read.RX";
                node_write_opcua_c_ry = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Read.RY";
                node_write_opcua_c_rz = "ns=6;s=::" + program_name + ":abb_control.Cartesian.Read.RZ";

                // -------------------- Main Cycle {While} -------------------- //
                while (true)
                {
                    Console.WriteLine("[INFO] Connect to RobotStudio ABB EGM (y/n):");
                    // Connect sdk - var
                    string connect_sdk = Convert.ToString(Console.ReadLine());

                    if (connect_sdk == "y")
                    {
                        // ------------------------ Threading Block { OPCUa Read Data } ------------------------//
                        opcua_c_r_while = true;

                        opcua_client_r_Thread = new Thread(() => OPCUa_r_thread_function(ip_adr_plc, port_adr_plc));
                        opcua_client_r_Thread.IsBackground = true;
                        opcua_client_r_Thread.Start();

                        // ------------------------ Threading Block { OPCUa Write Data } ------------------------//
                        opcua_c_w_while = true;

                        opcua_client_w_Thread = new Thread(() => OPCUa_w_thread_function(ip_adr_plc, port_adr_plc));
                        opcua_client_w_Thread.IsBackground = true;
                        opcua_client_w_Thread.Start();

                        // ------------------------ Threading Block { OPCUa Write Data } ------------------------//
                        egm_c_rw_while = true;

                        egm_client_rw = new Thread(() => EGM_rw_thread_function(port_adr_robot));
                        egm_client_rw.IsBackground = true;
                        egm_client_rw.Start();

                        Console.WriteLine("[INFO] Stop RobotStudio EGM ABB (y):");
                        // Stop sdk - var
                        string stop_rs = Convert.ToString(Console.ReadLine());

                        if (stop_rs == "y")
                        {
                            // application quit
                            break;
                        }
                    }
                    else if (connect_sdk == "n")
                    {
                        // application quit
                        break;
                    }

                    // Application quit
                    Application_Quit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // Application {Quit}
                Application_Quit();
            }

        }

        // ------------------------------------------------------------------------------------------------------------------------//
        // -------------------------------------------------------- FUNCTIONS -----------------------------------------------------//
        // ------------------------------------------------------------------------------------------------------------------------//

        // -------------------- Abort Threading Blocks -------------------- //
        static void Application_Quit()
        {
            try
            {
                // OPCUa Read Data
                opcua_c_r_while = false;
                // OPCUa Write Data
                opcua_c_w_while = false;
                // ABB Robotstudio EGM -> Read/Write Data
                egm_c_rw_while = false;

                // Abort threading block {OPCUA -> read data}
                if (opcua_client_r_Thread.IsAlive == true)
                {
                    opcua_client_r_Thread.Abort();
                }

                // Abort threading block {OPCUA -> write data}
                if (opcua_client_w_Thread.IsAlive == true)
                {
                    opcua_client_w_Thread.Abort();
                }

                // Abort threading block {EGM -> read/write data}
                if (egm_client_rw.IsAlive == true)
                {
                    egm_client_rw.Abort();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // ------------------------ Threading Block { EGM read/write data - main function } ------------------------//
        static void EGM_rw_thread_function(int port_adr)
        {
            UdpClient udp_server = null;
            // Create an udp server and listen on any address and the port {ABB Robot Port is set from the RobotStudio ABB}
            udp_server = new UdpClient(port_adr);

            var end_point = new IPEndPoint(IPAddress.Any, port_adr);

            while (egm_c_rw_while)
            {
                // Get the data from the robot
                var data = udp_server.Receive(ref end_point);

                if (data != null)
                {
                    // Initialization ABB Robot {EGM READ data (position, rotation)}
                    EgmRobot abb_robot = EgmRobot.CreateBuilder().MergeFrom(data).Build();

                    if (read_start_r_joint == true)
                    {
                        // Read Joint data {(0 - 5) -> for 6 joints robot}
                        node_read_egm_j1 = abb_robot.FeedBack.Joints.GetJoints(0).ToString();
                        node_read_egm_j2 = abb_robot.FeedBack.Joints.GetJoints(1).ToString();
                        node_read_egm_j3 = abb_robot.FeedBack.Joints.GetJoints(2).ToString();
                        node_read_egm_j4 = abb_robot.FeedBack.Joints.GetJoints(3).ToString();
                        node_read_egm_j5 = abb_robot.FeedBack.Joints.GetJoints(4).ToString();
                        node_read_egm_j6 = abb_robot.FeedBack.Joints.GetJoints(5).ToString();
                    }
                    else if (read_start_r_cartesian == true)
                    {
                        // Read Cartesian Position
                        // TCP Position {X, Y, Z}
                        node_read_egm_c_x = abb_robot.FeedBack.Cartesian.Pos.X.ToString();
                        node_read_egm_c_y = abb_robot.FeedBack.Cartesian.Pos.Y.ToString();
                        node_read_egm_c_z = abb_robot.FeedBack.Cartesian.Pos.Z.ToString();
                        // Euler Angles {RX, RY, RZ}
                        node_read_egm_c_rx = abb_robot.FeedBack.Cartesian.Euler.X.ToString();
                        node_read_egm_c_ry = abb_robot.FeedBack.Cartesian.Euler.Y.ToString();
                        node_read_egm_c_rz = abb_robot.FeedBack.Cartesian.Euler.Z.ToString();
                    }

                    // OPCUa read data {Start - Joint/Cartesian move}
                    if (read_start_joint == true || read_start_cartesian == true)
                    {
                        // Create a new EGM sensor message
                        EgmSensor.Builder egm_sensor = EgmSensor.CreateBuilder();
                        // Function
                        EMG_sensor_message(egm_sensor);

                        using (MemoryStream memory_stream = new MemoryStream())
                        {
                            // Sensor Message
                            EgmSensor sensor_message = egm_sensor.Build();
                            sensor_message.WriteTo(memory_stream);

                            // Send message to the ABB ROBOT {UDP}
                            int bytes_sent = udp_server.Send(memory_stream.ToArray(),(int)memory_stream.Length, end_point);
                            // Check sent data
                            if (bytes_sent < 0)
                            {
                                Console.WriteLine("Error!");
                            }
                        }
                    }
                }
            }
        }

        // ------------------------ Sensor Message { EGM } ------------------------//
        static void EMG_sensor_message(EgmSensor.Builder egm_s)
        {
            EgmHeader.Builder egm_hdr = new EgmHeader.Builder();
            egm_hdr.SetSeqno(sequence_number++)
               //Timestamp in milliseconds (can be used for monitoring delays)
               .SetTm((uint)DateTime.Now.Ticks)
               //Sent by sensor, MSGTYPE_DATA if sent from robot controller
               .SetMtype(EgmHeader.Types.MessageType.MSGTYPE_CORRECTION);

            egm_s.SetHeader(egm_hdr);

            // -------------------- Create EMG Sensor Data -------------------- //
            // Planning
            EgmPlanned.Builder planned_trajectory = new EgmPlanned.Builder();
            // Joint Position (0 - 5) -> for 6 joints robot
            EgmJoints.Builder joint_pos = new EgmJoints.Builder();
            // Cartesian Position
            EgmPose.Builder cartesian_pos = new EgmPose.Builder();
            // TCP Position {X, Y, Z}
            EgmCartesian.Builder tcp_p = new EgmCartesian.Builder();
            // Euler Angles {RX, RY, RZ}
            EgmEuler.Builder ea_p = new EgmEuler.Builder();


            // OPCUa read data {Start - Joint/Cartesian move}
            if (read_start_joint == true)
            {
                // Send data from OPCUa to the ABB Robot {Joints}
                joint_pos.AddJoints(read_opcua_j1);
                joint_pos.AddJoints(read_opcua_j2);
                joint_pos.AddJoints(read_opcua_j3);
                joint_pos.AddJoints(read_opcua_j4);
                joint_pos.AddJoints(read_opcua_j5);
                joint_pos.AddJoints(read_opcua_j6);
                // Planned EGM Trajectory
                planned_trajectory.SetJoints(joint_pos);
                // Set trajectory
                egm_s.SetPlanned(planned_trajectory);
            }
            else if (read_start_cartesian == true)
            {
                // TCP
                tcp_p.SetX(read_opcua_c_x)
                  .SetY(read_opcua_c_y)
                  .SetZ(read_opcua_c_z);
                // Eueler Angles
                ea_p.SetX(read_opcua_c_rx)
                    .SetY(read_opcua_c_ry)
                    .SetZ(read_opcua_c_rz);
                // Send data from OPCUa to the ABB Robot {Cartesian}
                cartesian_pos.SetPos(tcp_p).SetEuler(ea_p);
                // Planned EGM Trajectory
                planned_trajectory.SetCartesian(cartesian_pos);
                // Set trajectory
                egm_s.SetPlanned(planned_trajectory);
            }

            return;
        }

        // ------------------------ Threading Block { OPCUa Read Data } ------------------------//
        static void OPCUa_r_thread_function(string ip_adr, string port_adr)
        {
            // OPCUa client configuration
            client_configuration_r = opcua_client_configuration();
            // Establishing communication
            client_end_point_r = CoreClientUtils.SelectEndpoint("opc.tcp://" + ip_adr + ":" + port_adr, useSecurity: false, operationTimeout: 10000);
            // Create session
            client_session_r = opcua_create_session(client_configuration_r, client_end_point_r);

            // Threading while {read data}
            while (opcua_c_r_while)
            {
                // Read Data - BOOL {Start - Write Data}
                read_start_joint = bool.Parse(client_session_r.ReadValue(node_read_opcua_start_j).ToString());
                read_start_cartesian = bool.Parse(client_session_r.ReadValue(node_read_opcua_start_c).ToString());
                // Read Data - BOOL {Start - Read Data}
                read_start_r_joint = bool.Parse(client_session_r.ReadValue(node_read_opcua_start_r_j).ToString());
                read_start_r_cartesian = bool.Parse(client_session_r.ReadValue(node_read_opcua_start_r_c).ToString());
                // Read Data - Float {Joint}
                read_opcua_j1 = float.Parse(client_session_r.ReadValue(node_read_opcua_j1).ToString());
                read_opcua_j2 = float.Parse(client_session_r.ReadValue(node_read_opcua_j2).ToString());
                read_opcua_j3 = float.Parse(client_session_r.ReadValue(node_read_opcua_j3).ToString());
                read_opcua_j4 = float.Parse(client_session_r.ReadValue(node_read_opcua_j4).ToString());
                read_opcua_j5 = float.Parse(client_session_r.ReadValue(node_read_opcua_j5).ToString());
                read_opcua_j6 = float.Parse(client_session_r.ReadValue(node_read_opcua_j6).ToString());
                // Read Data - Float {Cartesian}
                read_opcua_c_x = float.Parse(client_session_r.ReadValue(node_read_opcua_c_x).ToString());
                read_opcua_c_y = float.Parse(client_session_r.ReadValue(node_read_opcua_c_y).ToString());
                read_opcua_c_z = float.Parse(client_session_r.ReadValue(node_read_opcua_c_z).ToString());
                read_opcua_c_rx = float.Parse(client_session_r.ReadValue(node_read_opcua_c_rx).ToString());
                read_opcua_c_ry = float.Parse(client_session_r.ReadValue(node_read_opcua_c_ry).ToString());
                read_opcua_c_rz = float.Parse(client_session_r.ReadValue(node_read_opcua_c_rz).ToString());
            }
        }

        // ------------------------ Threading Block { OPCUa Write Data } ------------------------//
        static void OPCUa_w_thread_function(string ip_adr, string port_adr)
        {
            // OPCUa client configuration
            client_configuration_w = opcua_client_configuration();
            // Establishing communication
            client_end_point_w = CoreClientUtils.SelectEndpoint("opc.tcp://" + ip_adr + ":" + port_adr, useSecurity: false, operationTimeout: 10000);
            // Create session
            client_session_w = opcua_create_session(client_configuration_w, client_end_point_w);

            // Threading while {write data}
            while (opcua_c_w_while)
            {
                // Write data {Joint - ABB ROBOT}
                if (node_read_egm_j1 != null && node_read_egm_j2 != null && node_read_egm_j3 != null && node_read_egm_j4 != null && node_read_egm_j5 != null && node_read_egm_j6 != null)
                {
                    opcua_write_value(client_session_w, node_write_opcua_j1, node_read_egm_j1);
                    opcua_write_value(client_session_w, node_write_opcua_j2, node_read_egm_j2);
                    opcua_write_value(client_session_w, node_write_opcua_j3, node_read_egm_j3);
                    opcua_write_value(client_session_w, node_write_opcua_j4, node_read_egm_j4);
                    opcua_write_value(client_session_w, node_write_opcua_j5, node_read_egm_j5);
                    opcua_write_value(client_session_w, node_write_opcua_j6, node_read_egm_j6);
                }
                // Write data {Cartesian - ABB ROBOT}
                if (node_read_egm_c_x != null && node_read_egm_c_y != null && node_read_egm_c_z != null && node_read_egm_c_rx != null && node_read_egm_c_ry != null && node_read_egm_c_rz != null)
                {
                    opcua_write_value(client_session_w, node_write_opcua_c_x, node_read_egm_c_x);
                    opcua_write_value(client_session_w, node_write_opcua_c_y, node_read_egm_c_y);
                    opcua_write_value(client_session_w, node_write_opcua_c_z, node_read_egm_c_z);
                    opcua_write_value(client_session_w, node_write_opcua_c_rx, node_read_egm_c_rx);
                    opcua_write_value(client_session_w, node_write_opcua_c_ry, node_read_egm_c_ry);
                    opcua_write_value(client_session_w, node_write_opcua_c_rz, node_read_egm_c_rz);
                }
            }
        }

        // ------------------------ OPCUa Client {Application -> Configuration (STEP 1)} ------------------------//
        static ApplicationConfiguration opcua_client_configuration()
        {
            // Configuration OPCUa Client {W/R -> Data}
            var config = new ApplicationConfiguration()
            {
                // Initialization (Name, Uri, etc.)
                ApplicationName = "OPCUa_AS", // OPCUa AS (Automation Studio B&R)
                ApplicationUri = Utils.Format(@"urn:{0}:OPCUa_AS", System.Net.Dns.GetHostName()),
                // Type -> Client
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    // Security Configuration - Certificate
                    ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = Utils.Format(@"CN={0}, DC={1}", "OPCUa_AS", System.Net.Dns.GetHostName()) },
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Certificate Authorities" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications" },
                    RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\RejectedCertificates" },
                    AutoAcceptUntrustedCertificates = true,
                    AddAppCertToTrustedStore = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 10000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 50000 },
                TraceConfiguration = new TraceConfiguration()
            };
            config.Validate(ApplicationType.Client).GetAwaiter().GetResult();
            if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                config.CertificateValidator.CertificateValidation += (s, e) => { e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted); };
            }

            var application = new ApplicationInstance
            {
                ApplicationName = "OPCUa_AS",
                ApplicationType = ApplicationType.Client,
                ApplicationConfiguration = config
            };
            application.CheckApplicationInstanceCertificate(false, 2048).GetAwaiter().GetResult();

            return config;
        }

        // ------------------------ OPCUa Client {Application -> Create Session (STEP 2)} ------------------------//
        static Session opcua_create_session(ApplicationConfiguration client_configuration, EndpointDescription client_end_point)
        {
            return Session.Create(client_configuration, new ConfiguredEndpoint(null, client_end_point, EndpointConfiguration.Create(client_configuration)), false, "", 10000, null, null).GetAwaiter().GetResult();
        }

        // ------------------------ OPCUa Client {Write Value (Define - Node)} ------------------------//
        static bool opcua_write_value(Session client_session, string node_id, string value_write)
        {
            // Initialization (Bide)
            NodeId init_node = NodeId.Parse(node_id);

            try
            {
                // Find Node (OPCUa Client)
                Node node = client_session.NodeCache.Find(init_node) as Node;
                DataValue init_data_value = client_session.ReadValue(node.NodeId);

                // Preparation data for writing
                WriteValue value = new WriteValue()
                {
                    NodeId = init_node,
                    AttributeId = Attributes.Value,
                    Value = new DataValue(new Variant(Convert.ChangeType(value_write, init_data_value.Value.GetType()))),
                };

                // Initialization (Write)
                WriteValueCollection init_write = new WriteValueCollection();
                // Append variable
                init_write.Add(value);

                StatusCodeCollection results = null;
                DiagnosticInfoCollection diagnosticInfos = null;

                // Wriate data
                client_session.Write(null, init_write, out results, out diagnosticInfos);

                // Check Result (Status)
                if (results[0] == StatusCodes.Good)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}
