/* Copyright (C) 2014 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SMBLibrary.Utilities;

namespace SMBLibrary.Services
{
    public abstract class RemoteService
    {
        public abstract byte[] GetResponseBytes(ushort opNum, byte[] requestBytes);

        public abstract Guid InterfaceGuid
        {
            get;
        }

        public abstract string PipeName
        {
            get;
        }
    }
}
