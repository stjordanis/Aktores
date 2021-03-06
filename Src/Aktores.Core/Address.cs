﻿namespace Aktores.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Address
    {
        private string protocol;
        private string systemname;
        private string hostname;
        private int port;

        public Address(string path)
        {
            int p = path.IndexOf(':');

            if (p > 0)
            {
                this.protocol = path.Substring(0, p);
                path = path.Substring(p + 1);
            }
            else
                this.protocol = "aktores.tcp";

            if (path.StartsWith("//"))
            {
                string location = path.Substring(2);

                p = location.IndexOf("@");

                if (p > 0)
                {
                    this.systemname = location.Substring(0, p);
                    location = location.Substring(p + 1);

                    p = location.IndexOf(":");

                    if (p > 0)
                    {
                        this.hostname = location.Substring(0, p);
                        this.port = int.Parse(location.Substring(p + 1));
                    }
                    else
                        this.hostname = location;
                }
                else
                {
                    this.systemname = location;
                    this.hostname = "localhost";
                }
            }
            else
                this.systemname = "sys";
        }

        public string Protocol { get { return this.protocol; } }

        public string SystemName { get { return this.systemname; } }

        public string HostName { get { return this.hostname; } }

        public int Port { get { return this.port; } }

        public static Address GetAddress(string path)
        {
            int p = path.IndexOf("//");

            if (p < 0)
                return null;

            var pa = path.IndexOf("/", p + 2);

            if (pa < 0)
                return new Address(path);

            return new Address(path.Substring(0, pa));
        }

        public static ActorPath GetActorPath(string path)
        {
            int p = path.IndexOf("//");

            if (p < 0)
                return new ActorPath(path);

            var pa = path.IndexOf("/", p + 2);

            if (pa < 0)
                return null;

            return new ActorPath(path.Substring(pa + 1));
        }

        public override string ToString()
        {
            string url = string.Format("{0}://{1}@{2}", this.protocol, this.systemname, this.hostname);

            if (this.port > 0)
                url = string.Format("{0}:{1}", url, this.port);

            return url;
        }
    }
}
