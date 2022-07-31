/*
Copyright (c) 2015 Harm Hanemaaijer <fgenfb@yahoo.com>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

namespace AssetsTools
{
    public static class DXTDecoders
    {
        public static byte[] ReadDXT1(byte[] data, int width, int height, bool dxt1a = false)
        {
            var blockCountX = (width + 3) >> 2;
            var blockCountY = (height + 3) >> 2;

            var len = blockCountX * blockCountY * 16 * 4;
            var bytes = new byte[len];

            var pos = 0;

            bool opaque;
            uint colors, code;
            var cr = new uint[4];
            var cg = new uint[4];
            var cb = new uint[4];
            int ca;
            var m3val = (byte)(dxt1a ? 0x00 : 0xFF);

            uint pix;

            int x, y, i;

            for (y = 0; y < blockCountY; y++)
            {
                for (x = 0; x < blockCountX; x++)
                {
                    colors = (uint)(data[pos] | (data[pos + 1] << 8) | (data[pos + 2] << 16) | (data[pos + 3] << 24));
                    code = (uint)(data[pos + 4] | (data[pos + 5] << 8) | (data[pos + 6] << 16) | (data[pos + 7] << 24));
                    opaque = (colors & 0xFFFF) > ((colors & 0xFFFF0000) >> 16);

                    cb[0] = (colors & 0x0000001F) << 3;
                    cg[0] = (colors & 0x000007E0) >> (5 - 2);
                    cr[0] = (colors & 0x0000F800) >> (11 - 3);
                    cb[1] = (colors & 0x001F0000) >> (16 - 3);
                    cg[1] = (colors & 0x07E00000) >> (21 - 2);
                    cr[1] = (colors & 0xF8000000) >> (27 - 3);
                    ca = 0xFF;

                    if (opaque)
                    {
                        cr[2] = DivisionTables.DivideBy3[2 * cr[0] + cr[1]];
                        cg[2] = DivisionTables.DivideBy3[2 * cg[0] + cg[1]];
                        cb[2] = DivisionTables.DivideBy3[2 * cb[0] + cb[1]];
                        cr[3] = DivisionTables.DivideBy3[cr[0] + 2 * cr[1]];
                        cg[3] = DivisionTables.DivideBy3[cg[0] + 2 * cg[1]];
                        cb[3] = DivisionTables.DivideBy3[cb[0] + 2 * cb[1]];
                    }
                    else
                    {
                        cr[2] = (cr[0] + cr[1]) / 2;
                        cg[2] = (cg[0] + cg[1]) / 2;
                        cb[2] = (cb[0] + cb[1]) / 2;
                        cr[3] = 0x00;
                        cg[3] = 0x00;
                        cb[3] = 0x00;
                        ca = m3val;
                    }

                    for (i = 0; i < 16; i++)
                    {
                        pix = (code >> (i * 2)) & 0x3;

                        var dataPos = (x * 4 * 4) + (i % 4 * 4) + (y * width * 4 * 4) + ((i >> 2) * width * 4);
                        bytes[dataPos] = (byte)cb[pix];
                        bytes[dataPos + 1] = (byte)cg[pix];
                        bytes[dataPos + 2] = (byte)cr[pix];
                        bytes[dataPos + 3] = (byte)ca;
                    }
                    pos += 8;
                }
            }
            return bytes;
        }

        public static byte[] ReadDXT5(byte[] data, int width, int height)
        {
            var blockCountX = (width + 3) >> 2;
            var blockCountY = (height + 3) >> 2;

            var len = blockCountX * blockCountY * 16 * 4;
            var bytes = new byte[len];

            var pos = 0;

            int a0, a1;
            ulong alpha;
            uint colors, code;
            var cr = new uint[4];
            var cg = new uint[4];
            var cb = new uint[4];
            int ca;

            uint pix;
            int alphaCode;

            int x, y, i;

            for (y = 0; y < blockCountY; y++)
            {
                for (x = 0; x < blockCountX; x++)
                {
                    a0 = data[pos];
                    a1 = data[pos + 1];

                    alpha = data[pos + 2] | ((ulong)data[pos + 3] << 8) | ((ulong)data[pos + 4] << 16) |
                        ((ulong)data[pos + 5] << 24) | ((ulong)data[pos + 6] << 32) | ((ulong)data[pos + 7] << 40);
                    colors = (uint)(data[pos + 8] | (data[pos + 9] << 8) | (data[pos + 10] << 16) | (data[pos + 11] << 24));
                    code = (uint)(data[pos + 12] | (data[pos + 13] << 8) | (data[pos + 14] << 16) | (data[pos + 15] << 24));

                    cb[0] = (colors & 0x0000001F) << 3;
                    cg[0] = (colors & 0x000007E0) >> (5 - 2);
                    cr[0] = (colors & 0x0000F800) >> (11 - 3);
                    cb[1] = (colors & 0x001F0000) >> (16 - 3);
                    cg[1] = (colors & 0x07E00000) >> (21 - 2);
                    cr[1] = (colors & 0xF8000000) >> (27 - 3);
                    cr[2] = DivisionTables.DivideBy3[(cr[0] << 1) + cr[1]];
                    cg[2] = DivisionTables.DivideBy3[(cg[0] << 1) + cg[1]];
                    cb[2] = DivisionTables.DivideBy3[(cb[0] << 1) + cb[1]];
                    cr[3] = DivisionTables.DivideBy3[cr[0] + (cr[1] << 1)];
                    cg[3] = DivisionTables.DivideBy3[cg[0] + (cg[1] << 1)];
                    cb[3] = DivisionTables.DivideBy3[cb[0] + (cb[1] << 1)];

                    for (i = 0; i < 16; i++)
                    {
                        pix = (code >> (i * 2)) & 0x3;

                        alphaCode = (int)((alpha >> (i * 3)) & 0x7);

                        if (a0 > a1)
                        {
                            ca = alphaCode switch
                            {
                                0 => a0,
                                1 => a1,
                                2 => DivisionTables.DivideBy7[6 * a0 + 1 * a1],
                                3 => DivisionTables.DivideBy7[5 * a0 + 2 * a1],
                                4 => DivisionTables.DivideBy7[4 * a0 + 3 * a1],
                                5 => DivisionTables.DivideBy7[3 * a0 + 4 * a1],
                                6 => DivisionTables.DivideBy7[2 * a0 + 5 * a1],
                                7 => DivisionTables.DivideBy7[1 * a0 + 6 * a1],
                                _ => 0,
                            };
                        }
                        else
                        {
                            ca = alphaCode switch
                            {
                                0 => a0,
                                1 => a1,
                                2 => DivisionTables.DivideBy5[4 * a0 + 1 * a1],
                                3 => DivisionTables.DivideBy5[3 * a0 + 2 * a1],
                                4 => DivisionTables.DivideBy5[2 * a0 + 3 * a1],
                                5 => DivisionTables.DivideBy5[1 * a0 + 4 * a1],
                                6 => 0,
                                7 => 0xFF,
                                _ => 0,
                            };
                        }

                        var dataPos = (x * 4 * 4) + (i % 4 * 4) + (y * width * 4 * 4) + ((i >> 2) * width * 4);
                        bytes[dataPos] = (byte)cb[pix];
                        bytes[dataPos + 1] = (byte)cg[pix];
                        bytes[dataPos + 2] = (byte)cr[pix];
                        bytes[dataPos + 3] = (byte)ca;
                    }
                    pos += 16;
                }
            }
            return bytes;
        }
    }
}
