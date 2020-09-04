using System;

namespace CodeTiger
{
    /// <summary>
    /// Provides constants used to mask certain bits of integer values.
    /// </summary>
    public static class Bitmask
    {
        /// <summary>
        /// A bit mask that includes the lowest 8 bits (0 through 7) of an <see cref="short"/> value.
        /// </summary>
        public const short Int16Lower8 = 0x00ff;

        /// <summary>
        /// A bit mask that includes the highest 8 bits (8 through 15) of an <see cref="short"/> value.
        /// </summary>
        public const short Int16Upper8 = unchecked((short)0xff00);

        /// <summary>
        /// A bit mask that includes the lower 16 bits (8 through 15) of an <see cref="int"/> value.
        /// </summary>
        public const int Int32Lower16 = 0x0000ffff;

        /// <summary>
        /// A bit mask that includes the upper 16 bits (16 through 31) of an <see cref="int"/> value.
        /// </summary>
        public const int Int32Upper16 = unchecked((int)0xffff0000);

        /// <summary>
        /// A bit mask that includes bits 0 through 7 (8 bits total) of an <see cref="int"/> value.
        /// </summary>
        public const int Int32Bits0To7 = 0x000000ff;

        /// <summary>
        /// A bit mask that includes bits 8 through 15 (8 bits total) of an <see cref="int"/> value.
        /// </summary>
        public const int Int32Bits8To15 = (int)0x0000ff00;

        /// <summary>
        /// A bit mask that includes bits 16 through 23 (8 bits total) of an <see cref="int"/> value.
        /// </summary>
        public const int Int32Bits16To23 = (int)0x00ff0000;

        /// <summary>
        /// A bit mask that includes bits 24 through 31 (8 bits total) of an <see cref="int"/> value.
        /// </summary>
        public const int Int32Bits24To31 = unchecked((int)0xff000000);

        /// <summary>
        /// A bit mask that includes the lower 32 bits (0 through 31) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Lower32 = 0x00000000ffffffffL;

        /// <summary>
        /// A bit mask that includes the upper 32 bits (32 through 63) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Upper32 = unchecked((long)0xffffffff00000000L);

        /// <summary>
        /// A bit mask that includes bits 0 through 15 (16 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits0To15 = 0x000000000000ffffL;

        /// <summary>
        /// A bit mask that includes bits 16 through 31 (16 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits16To31 = 0x00000000ffff0000L;

        /// <summary>
        /// A bit mask that includes bits 32 through 47 (16 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits32To47 = 0x0000ffff00000000L;

        /// <summary>
        /// A bit mask that includes bits 48 through 63 (16 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits48To63 = unchecked((long)0xffff000000000000L);

        /// <summary>
        /// A bit mask that includes bits 0 through 7 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits0To7 = 0x00000000000000ffL;

        /// <summary>
        /// A bit mask that includes bits 8 through 15 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits8To15 = 0x000000000000ff00L;

        /// <summary>
        /// A bit mask that includes bits 16 through 23 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits16To23 = 0x0000000000ff0000L;

        /// <summary>
        /// A bit mask that includes bits 24 through 31 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits24To31 = 0x00000000ff000000L;

        /// <summary>
        /// A bit mask that includes bits 32 through 39 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits32To39 = 0x000000ff00000000L;

        /// <summary>
        /// A bit mask that includes bits 40 through 47 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits40To47 = 0x0000ff0000000000L;

        /// <summary>
        /// A bit mask that includes bits 48 through 55 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits48To55 = 0x00ff000000000000L;

        /// <summary>
        /// A bit mask that includes bits 56 through 63 (8 bits total) of an <see cref="long"/> value.
        /// </summary>
        public const long Int64Bits56To63 = unchecked((long)0xff00000000000000L);
    }
}
