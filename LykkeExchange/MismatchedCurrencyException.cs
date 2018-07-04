using System;

namespace LykkeExchange
{
    /// <inheritdoc />
    /// <summary>
    /// Exception when Money operations are performed on different currencies.
    /// </summary>
    /// <seealso cref="T:System.Exception" />
    internal class MismatchedCurrencyException : Exception
    {
        /// <summary>
        /// Gets the money1.
        /// </summary>
        /// <value>
        /// The money1.
        /// </value>
        public LykkeMoney Money1 { get; }
        /// <summary>
        /// Gets the money2.
        /// </summary>
        /// <value>
        /// The money2.
        /// </value>
        public LykkeMoney Money2 { get; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LiveTree.Platform.Web.SeedModel.Utils.Currency.MismatchedCurrencyException" /> class.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        public MismatchedCurrencyException(LykkeMoney a, LykkeMoney b) : base($"{a} and {b} have different currencies")
        {
            this.Money1 = a;
            this.Money2 = b;
        }
    }
}
