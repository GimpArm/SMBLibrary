/* Copyright (C) 2014-2019 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using System.Collections.Generic;
using System.Text;
using SMBLibrary.Utilities;

namespace SMBLibrary.SMB1
{
    /// <summary>
    /// TRANS_READ_NMPIPE Request
    /// </summary>
    public class TransactionReadNamedPipeResponse : TransactionSubcommand
    {
        public const int ParametersLength = 0;
        // Data:
        public byte[] ReadData;

        public TransactionReadNamedPipeResponse() : base()
        {
        }

        public TransactionReadNamedPipeResponse(byte[] data) : base()
        {
            ReadData = data;
        }

        public override byte[] GetData(bool isUnicode)
        {
            return ReadData;
        }

        public override TransactionSubcommandName SubcommandName
        {
            get
            {
                return TransactionSubcommandName.TRANS_READ_NMPIPE;
            }
        }
    }
}
