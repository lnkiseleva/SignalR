// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    public class HttpWebRequestWrapper : IRequest
    {
        private readonly IDictionary<string, string> _headerDictionary = new Dictionary<string, string>();
        private readonly HttpWebRequest _request;
        private HashSet<string> _restrictedHeaders;

        private string[] _restrictedHeaderName = { HttpRequestHeader.Accept.ToString(), HttpRequestHeader.Connection.ToString(), HttpRequestHeader.ContentType.ToString(),
                                         HttpRequestHeader.Date.ToString(), HttpRequestHeader.Expect.ToString(), HttpRequestHeader.Host.ToString(),
                                         HttpRequestHeader.IfModifiedSince.ToString(), HttpRequestHeader.Range.ToString(), HttpRequestHeader.Referer.ToString()};

        private IDictionary<string, Action<HttpWebRequest, string>> _restrictedHeaderDict = new Dictionary<string, Action<HttpWebRequest, string>>() {
                                                                        { HttpRequestHeader.Accept.ToString(), (request, value) => { request.Accept = value; } },
                                                                        { HttpRequestHeader.Connection.ToString(), (request, value) => { request.Connection = value; } },
                                                                        { HttpRequestHeader.ContentType.ToString(), (request, value) => { request.ContentType = value; } },
                                                                        { HttpRequestHeader.ContentLength.ToString(), (request, value) => { request.ContentLength = Int32.Parse(value); } },                                                                        
                                                                    };

        public HttpWebRequestWrapper(HttpWebRequest request)
        {
            _request = request;
            
            _restrictedHeaders = new HashSet<string>(_restrictedHeaderName);
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

        public IDictionary<string, string> Headers
        {
            // Add logic to check the type of the header and see if it needs to set separately
            get
            {
                if (_headerDictionary.Count == 0)
                {
                    PopulateHeaderDict();
                }
                return _headerDictionary;
            }
            set
            {
                SetRequestHeader(value);
            }
        }

        private void PopulateHeaderDict()
        {
            foreach (KeyValuePair<string, string> entry in _request.Headers)
            {
                _headerDictionary.Add(entry.Key, entry.Value);
            }

            object[] restrictedHeaderVar = {_request.Accept, _request.Connection, _request.ContentLength, _request.ContentType, _request.Date, _request.Expect, _request.Host, _request.IfModifiedSince,
                                           _request.Referer,_request.TransferEncoding, _request.UserAgent};

            for (int i = 0; i < restrictedHeaderVar.Length; i++)
            {
                if (restrictedHeaderVar[i] != null)
                {
                    _headerDictionary.Add(_restrictedHeaderName[i], (string) restrictedHeaderVar[i]);
                }
            }
        }

        private void SetRequestHeader(IDictionary<string, string> dictHeader)
        {
            foreach (KeyValuePair<string, string> headerEntry in dictHeader)
            {
                if (!_restrictedHeaders.Contains(headerEntry.Key))
                {
                    _request.Headers.Add(headerEntry.Key, headerEntry.Value);
                }
            }

            string accept;
            dictHeader.TryGetValue(HttpRequestHeader.Accept.ToString(), out accept);
            _request.Accept = accept;

            string connection;
            dictHeader.TryGetValue(HttpRequestHeader.Connection.ToString(), out connection);
            _request.Connection = connection;

            string contentType;
            dictHeader.TryGetValue(HttpRequestHeader.ContentType.ToString(), out contentType);
            _request.ContentType = contentType;

        }

        //public X509CertificateCollection ClientCertificates
        //{
        //    get
        //    {
        //        return _request.ClientCertificates;
        //    }
        //    set
        //    {
        //        _request.ClientCertificates = value;
        //    }
        //}

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
