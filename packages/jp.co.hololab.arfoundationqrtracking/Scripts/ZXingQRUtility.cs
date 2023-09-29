using System;
using System.Collections;
using System.Collections.Generic;
using ZXing;

namespace HoloLab.ARFoundationQRTracking
{
    public static class ZXingQRUtility
    {
        public static bool TryGetVersion(Result result, out int version)
        {
            var metadata = result.ResultMetadata;
            if (metadata == null)
            {
                version = 0;
                return false;
            }

            if (metadata.TryGetValue(ResultMetadataType.ERROR_CORRECTION_LEVEL, out var errorCorrectionLevel) == false)
            {
                version = 0;
                return false;
            }

            return TryFindVersion(result.RawBytes.Length, errorCorrectionLevel.ToString(), out version);
        }

        public static bool TryGetCellSize(Result result, out int cellSize)
        {
            if (TryGetVersion(result, out var version) == false)
            {
                cellSize = 0;
                return false;
            }

            cellSize = version * 4 + 17;
            return true;
        }

        private static bool TryFindVersion(int dataByteSize, string errorCorrectionLevel, out int version)
        {
            version = errorCorrectionLevel switch
            {
                "L" => dataByteSize switch
                {
                    19 => 1,
                    34 => 2,
                    55 => 3,
                    80 => 4,
                    108 => 5,
                    136 => 6,
                    156 => 7,
                    194 => 8,
                    232 => 9,
                    274 => 10,
                    324 => 11,
                    370 => 12,
                    428 => 13,
                    461 => 14,
                    523 => 15,
                    589 => 16,
                    647 => 17,
                    721 => 18,
                    795 => 19,
                    861 => 20,
                    932 => 21,
                    1006 => 22,
                    1094 => 23,
                    1174 => 24,
                    1276 => 25,
                    1370 => 26,
                    1468 => 27,
                    1531 => 28,
                    1631 => 29,
                    1735 => 30,
                    1843 => 31,
                    1955 => 32,
                    2071 => 33,
                    2191 => 34,
                    2306 => 35,
                    2434 => 36,
                    2566 => 37,
                    2702 => 38,
                    2812 => 39,
                    2956 => 40,
                    _ => 0,
                },
                "M" => dataByteSize switch
                {
                    16 => 1,
                    28 => 2,
                    44 => 3,
                    64 => 4,
                    86 => 5,
                    108 => 6,
                    124 => 7,
                    154 => 8,
                    182 => 9,
                    216 => 10,
                    254 => 11,
                    290 => 12,
                    334 => 13,
                    365 => 14,
                    415 => 15,
                    453 => 16,
                    507 => 17,
                    563 => 18,
                    627 => 19,
                    669 => 20,
                    714 => 21,
                    782 => 22,
                    860 => 23,
                    914 => 24,
                    1000 => 25,
                    1062 => 26,
                    1128 => 27,
                    1193 => 28,
                    1267 => 29,
                    1373 => 30,
                    1455 => 31,
                    1541 => 32,
                    1631 => 33,
                    1725 => 34,
                    1812 => 35,
                    1914 => 36,
                    1992 => 37,
                    2102 => 38,
                    2216 => 39,
                    2334 => 40,
                    _ => 0,
                },
                "Q" => dataByteSize switch
                {
                    13 => 1,
                    22 => 2,
                    34 => 3,
                    48 => 4,
                    62 => 5,
                    76 => 6,
                    88 => 7,
                    110 => 8,
                    132 => 9,
                    154 => 10,
                    180 => 11,
                    206 => 12,
                    244 => 13,
                    261 => 14,
                    295 => 15,
                    325 => 16,
                    367 => 17,
                    397 => 18,
                    445 => 19,
                    485 => 20,
                    512 => 21,
                    568 => 22,
                    614 => 23,
                    664 => 24,
                    718 => 25,
                    754 => 26,
                    808 => 27,
                    871 => 28,
                    911 => 29,
                    985 => 30,
                    1033 => 31,
                    1115 => 32,
                    1171 => 33,
                    1231 => 34,
                    1286 => 35,
                    1354 => 36,
                    1426 => 37,
                    1502 => 38,
                    1582 => 39,
                    1666 => 40,
                    _ => 0,
                },
                "H" => dataByteSize switch
                {
                    9 => 1,
                    16 => 2,
                    26 => 3,
                    36 => 4,
                    46 => 5,
                    60 => 6,
                    66 => 7,
                    86 => 8,
                    100 => 9,
                    122 => 10,
                    140 => 11,
                    158 => 12,
                    180 => 13,
                    197 => 14,
                    223 => 15,
                    253 => 16,
                    283 => 17,
                    313 => 18,
                    341 => 19,
                    385 => 20,
                    406 => 21,
                    442 => 22,
                    464 => 23,
                    514 => 24,
                    538 => 25,
                    596 => 26,
                    628 => 27,
                    661 => 28,
                    701 => 29,
                    745 => 30,
                    793 => 31,
                    845 => 32,
                    901 => 33,
                    961 => 34,
                    986 => 35,
                    1054 => 36,
                    1096 => 37,
                    1142 => 38,
                    1222 => 39,
                    1276 => 40,
                    _ => 0,
                },
                _ => 0,
            };

            return version != 0;
        }
    }
}
