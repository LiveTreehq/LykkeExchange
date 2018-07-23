using System;

namespace ExchangeMarket
{
    /// <summary>
    /// Money pattern implementation (according to Patterns of Enterprise Application Architecture (Fowler, 2002))
    /// Fowler, M. (2002). Patterns of enterprise application architecture. Addison-Wesley Longman Publishing Co., Inc.
    /// https://martinfowler.com/books/eaa.html
    /// https://martinfowler.com/eaaCatalog/money.html
    /// </summary>
    public struct LykkeMoney : IEquatable<LykkeMoney>
    {
        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }
        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LykkeMoney"/> struct.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        public LykkeMoney(decimal amount, string currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var money = obj is LykkeMoney ? (LykkeMoney)obj : new LykkeMoney();
            return obj != null && this.Equals(money);
        }
        /// <inheritdoc />
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(LykkeMoney other) => this.Amount == other.Amount && this.Currency == other.Currency;
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => unchecked((this.Amount.GetHashCode() * 397) ^ this.Currency.GetHashCode());

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static LykkeMoney operator +(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);

            return new LykkeMoney(a.Amount + b.Amount, a.Currency);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static LykkeMoney operator -(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);

            return new LykkeMoney(a.Amount - b.Amount, a.Currency);
        }

        /// <summary>
        /// Implements the unary operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static LykkeMoney operator -(LykkeMoney a) => new LykkeMoney(-a.Amount, a.Currency);

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="f">f.</param>
        /// <param name="m">m.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// 
        public static LykkeMoney operator *(decimal f, LykkeMoney m) => new LykkeMoney(f * m.Amount, m.Currency);
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="m">m.</param>
        /// <param name="f">f.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static LykkeMoney operator *(LykkeMoney m, decimal f) => new LykkeMoney(f * m.Amount, m.Currency);

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="m">m.</param>
        /// <param name="f">f.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static LykkeMoney operator /(LykkeMoney m, decimal f) => new LykkeMoney(m.Amount / f, m.Currency);

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static bool operator ==(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);
            return a.Amount == b.Amount;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static bool operator !=(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);
            return a.Amount != b.Amount;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static bool operator <(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);
            return a.Amount < b.Amount;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static bool operator >(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);
            return a.Amount > b.Amount;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static bool operator <=(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);
            return a.Amount <= b.Amount;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        /// <exception cref="MismatchedCurrencyException"></exception>
        public static bool operator >=(LykkeMoney a, LykkeMoney b)
        {
            if (a.Currency != b.Currency) throw new MismatchedCurrencyException(a, b);
            return a.Amount >= b.Amount;
        }


        /// <summary>
        /// Convert to string.
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() => $"{this.Amount} {this.Currency}";
    }
}
