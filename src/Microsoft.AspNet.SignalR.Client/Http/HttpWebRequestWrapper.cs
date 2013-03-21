﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System.Collections.Generic;
using System.Net;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    public class HttpWebRequestWrapper : IRequest
    {
        private readonly HttpWebRequest _request;
        private HashSet<HttpRequestHeader> _restrictedHeaders;

        public HttpWebRequestWrapper(HttpWebRequest request)
        {
            _request = request;
            HttpRequestHeader[] requestArray = { HttpRequestHeader.Accept, HttpRequestHeader.ContentType};
            _restrictedHeaders = new HashSet<HttpRequestHeader>(requestArray);
        }

        public string UserAgent
        {
            get
            {
                return _request.UserAgent;
            }
            set
            {
                _request.UserAgent = value;
            }
        }

        public ICredentials Credentials
        {
            get
            {
                return _request.Credentials;
            }
            set
            {
                _request.Credentials = value;
            }
        }

        public CookieContainer CookieContainer
        {
            get
            {
                return _request.CookieContainer;
            }
            set
            {
                _request.CookieContainer = value;
            }
        }

        public WebHeaderCollection Headers
        {
            // Add logic to check the type of the header and see if it needs to set separately
            get
            {
                return _request.Headers;
            }
            set
            {
                _request.Headers = value;
            }
        }

        public X509CertificateCollection ClientCertificates
        {
            get
            {
                return _request.ClientCertificates;
            }
            set
            {
                _request.ClientCertificates = value;
            }
        }

        public string Accept
        {
            get
            {
                return _request.Accept;
            }
            set
            {
                _request.Accept = value;
            }
        }

#if !SILVERLIGHT
        public IWebProxy Proxy
        {
            get
            {
                return _request.Proxy;
            }
            set
            {
                _request.Proxy = value;
            }
        }
#endif

        public void Abort()
        {
            _request.Abort();
        }
    }
}
