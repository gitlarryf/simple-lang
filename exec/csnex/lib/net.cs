using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace csnex.rtl
{
    public class SocketObject: Object
    {
        public Socket handle;
        public SocketObject(Socket s)
        {
            handle = s;
        }

        ~SocketObject()
        {
            if (handle != null) {
                handle.Close();
                handle.Dispose();
            }
        }

        public override string toString()
        {
            return string.Format("<Socket:{0}>", handle.ToString());
        }
    }

    public class net
    {
        private readonly Executor Exec;
        public net(Executor exe)
        {
            Exec = exe;
        }

        private Socket check_socket(object ps)
        {
            SocketObject so = (SocketObject)ps;
            if (so == null || so.handle == null) {
                throw new NeonRuntimeException("SocketException", "");
            }
            return so.handle;
        }

        public void socket_tcpSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Exec.stack.Push(Cell.CreateObjectCell(new SocketObject(s)));
        }

        public void socket_udpSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Exec.stack.Push(Cell.CreateObjectCell(new SocketObject(s)));
        }

        public void socket_accept()
        {
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }

            try {
                Socket r = ps.Accept();
                Exec.stack.Push(Cell.CreateObjectCell(new SocketObject(r)));
            } catch (SocketException) {
                Exec.stack.Push(Cell.CreateObjectCell(null));
            }
        }

        public void socket_bind()
        {
            Number port = Exec.stack.Pop().Number;
            string address = Exec.stack.Pop().String;
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }

            try {
                int p = Number.number_to_int32(port);
                if (address.Length == 0) {
                    address = Dns.GetHostName();
                }
                // ToDo: Figure out how/who to bind to.
                //IPHostEntry ipHost = Dns.GetHostEntry(address);
                //IPAddress ipAddr = ipHost.AddressList[0];
                //if (ipAddr == IPAddress.None) {
                //    ipHost = Dns.GetHostEntry(address);
                //}
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, p);
                ps.Bind(localEndPoint);
            } catch (SocketException) {
            }
        }

        public void socket_close()
        {
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }

            ps.Close();
        }

        public void socket_connect()
        {
            Number port = Exec.stack.Pop().Number;
            string host = Exec.stack.Pop().String;
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }

            int p = Number.number_to_int32(port);
            try {
                ps.Connect(host, p);
            } catch (SocketException) {
            }
        }

        public void socket_listen()
        {
            Number port = Exec.stack.Pop().Number;
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }

            ps.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            int p = Number.number_to_int32(port);
            try {
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, p);
                ps.Bind(localEndPoint);
                ps.Listen(5);
            } catch (SocketException) {
            }
        }

        public void socket_recv()
        {
            Number count = Exec.stack.Pop().Number;
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }

            try {
                int n = Number.number_to_int32(count);
                byte[] buf = new byte[n];
                SocketError sockerror = 0;
                int r = ps.Receive(buf, 0, n, SocketFlags.None, out sockerror);
                if (r < 0) {
                    Exec.stack.Push(Cell.CreateBytesCell(new byte[0]));
                    return;
                }
                if (r == 0) {
                    Exec.stack.Push(Cell.CreateBooleanCell(false));
                    Exec.stack.Push(Cell.CreateBytesCell(new byte[0]));
                    return;
                }
                Exec.stack.Push(Cell.CreateBooleanCell(true));
                byte[] ret = new byte[r];
                Array.Copy(buf, ret, r);
                Exec.stack.Push(Cell.CreateBytesCell(ret));
            } catch (SocketException) {
                Exec.stack.Push(Cell.CreateBooleanCell(false));
                Exec.stack.Push(Cell.CreateBytesCell(new byte[0]));
            }
        }

        // ToDo: *** Test this routine!! ***
        public void socket_recvfrom()
        {
            byte[] buffer = Exec.stack.Pop().Bytes;
            Number port = Exec.stack.Pop().Number;
            string address = Exec.stack.Pop().String;
            Number count = Exec.stack.Pop().Number;
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }

            try {
                int size = Number.number_to_int32(count);
                Array.Resize(ref buffer, size);
                IPHostEntry ipHost = Dns.GetHostEntry(address);
                IPAddress ipAddr = IPAddress.Any;
                if (ipAddr == IPAddress.None) {
                    ipHost = Dns.GetHostEntry(address);
                }
                IPEndPoint IPRemote = new IPEndPoint(IPAddress.Any, Number.number_to_int32(port));
                EndPoint RemoteEP = (EndPoint)IPRemote;
                int n = Number.number_to_int32(count);
                int r = ps.ReceiveFrom(buffer, size, SocketFlags.None, ref RemoteEP);
                if (r < 0) {
                    buffer = new byte[0];
                    // Shouldn't we raise an exception here?
                    Exec.stack.Push(Cell.CreateStringCell((RemoteEP as IPEndPoint).Address.ToString()));
                    Exec.stack.Push(Cell.CreateNumberCell(new Number((RemoteEP as IPEndPoint).Port)));
                    Exec.stack.Push(Cell.CreateBytesCell(buffer));
                    Exec.stack.Push(Cell.CreateBooleanCell(false));
                    return;
                }
                if (r == 0) {
                    buffer = new byte[0];
                    Exec.stack.Push(Cell.CreateStringCell((RemoteEP as IPEndPoint).Address.ToString()));
                    Exec.stack.Push(Cell.CreateNumberCell(new Number((RemoteEP as IPEndPoint).Port)));
                    Exec.stack.Push(Cell.CreateBytesCell(buffer));
                    Exec.stack.Push(Cell.CreateBooleanCell(false));
                    return;
                }
                Array.Resize(ref buffer, r);
                Exec.stack.Push(Cell.CreateStringCell((RemoteEP as IPEndPoint).Address.ToString()));
                Exec.stack.Push(Cell.CreateNumberCell(new Number((RemoteEP as IPEndPoint).Port)));
                Exec.stack.Push(Cell.CreateBytesCell(buffer));
                Exec.stack.Push(Cell.CreateBooleanCell(true));
            } catch (SocketException) {
                buffer = new byte[0];
                Exec.stack.Push(Cell.CreateBooleanCell(false));
            }
        }

        public void socket_send()
        {
            byte[] data = Exec.stack.Pop().Bytes;
            Object o = Exec.stack.Pop().Object;
            Socket ps = check_socket(o);
            if (ps == null) {
                return;
            }
            ps.Send(data, data.Length, SocketFlags.None);
        }

        public void socket_select()
        {
            Number timeout_seconds = Exec.stack.Pop().Number;
            Cell error = Exec.stack.Pop().Address;
            Cell write = Exec.stack.Pop().Address;
            Cell read = Exec.stack.Pop().Address;

            List<Socket> rfds = new List<Socket>();
            List<Socket> wfds = new List<Socket>();
            List<Socket> efds = new List<Socket>();

            for (int i = 0; i < read.Array.Count; i++) {
                Socket ps = check_socket(read.Array[i].Array[0].Object);
                rfds.Add(ps);
            }

            for (int i = 0; i < write.Array.Count; i++) {
                Socket ps = check_socket(write.Array[i].Array[0].Object);
                wfds.Add(ps);
            }

            for (int i = 0; i < error.Array.Count; i++) {
                Socket ps = check_socket(error.Array[i].Array[0].Object);
                efds.Add(ps);
            }

            int tv = 0;
            if (!timeout_seconds.IsNegative()) {
                tv = Number.number_to_int32(Number.Modulo(Number.Multiply(timeout_seconds, new Number(1000000)), new Number(1000000)));
            }

            try {
                Socket.Select(rfds, wfds, efds, tv);

                if (rfds.Count == 0 && wfds.Count == 0 && efds.Count == 0) {
                    read.Array.Clear();
                    write.Array.Clear();
                    error.Array.Clear();
                    Exec.stack.Push(Cell.CreateBooleanCell(false));
                    return;
                }

                for (int i = 0; i < read.Array.Count; ) {
                    Cell s = read.Array[i];
                    Socket ps = check_socket(s.Array[0].Object);
                    if (rfds.Exists(soc => soc.Equals(ps))) {
                        ++i;
                    } else {
                        read.Array.Remove(s);
                    }
                }
                for (int i = 0; i < write.Array.Count; ) {
                    Cell s = write.Array[i];
                    Socket ps = check_socket(s.Array[0].Object);
                    if (wfds.Exists(soc => soc.Equals(ps))) {
                        ++i;
                    } else {
                        write.Array.Remove(s);
                    }
                }
                for (int i = 0; i < error.Array.Count; ) {
                    Cell s = error.Array[i];
                    Socket ps = check_socket(s.Array[0].Object);
                    if (efds.Exists(soc => soc.Equals(ps))) {
                        ++i;
                    } else {
                        error.Array.Remove(s);
                    }
                }
            } catch (SocketException sex) {
                throw new NeonRuntimeException("SocketException", sex.SocketErrorCode.ToString());
            }
            Exec.stack.Push(Cell.CreateBooleanCell(true));
        }
    }
}
