// See https://aka.ms/new-console-template for more information
using HashidsNet;
using Nanoid;

try
{
  var email = new Email("");
}
catch (Exception)
{
  Console.WriteLine("Validation Worked for email");
}

try
{
  _ = new TransactionId(0, "12345");
}
catch (Exception)
{
  Console.WriteLine("Validation Worked for TrnId");
}
var trnId = new TransactionId(5001, Nanoid.Nanoid.Generate());
var trnHash = trnId.ToString();
var convertedTrndId = (TransactionId)trnHash;

Console.WriteLine(convertedTrndId);
Console.WriteLine("Are the 2 object values the same, {0}", (convertedTrndId == trnId));

Console.ReadKey();

public record Email
{
  public static bool Validate(string email) => string.IsNullOrEmpty(email) ? throw new ArgumentException() : true;
  public Email(string email)
  {
    Validate(email);
    Value = email;
  }

  public string Value { get; init; }
}

public record TransactionId(int ProductId, string TransactionNumber)
{
  private static readonly Hashids hashids = new Hashids("this is my salt");
  public bool validate = (ProductId > 0) ? true : throw new ArgumentException(nameof(ProductId));
  private string hasid = null;

  public static explicit operator TransactionId(string hashId)
  {
    var str = System.Text.Encoding.UTF8.GetString(Convert.FromHexString(hashids.DecodeHex(hashId)));
    //var str = hashids.DecodeHex(hashId)
    //  .Chunk(2)
    //  .Select(x => (char)Convert.ToByte(new string(x), 16))
    //  .Aggregate(string.Empty, (x, y) => x += y);
    var split = str.Split('|');
    return new TransactionId(int.Parse(split.First()), split.Last());
  }

  public override string ToString()
  {
    if (hasid == null)
    {
      byte[] arr = $"{ProductId}|{TransactionNumber.Replace("-", "")}".Select(x=> (byte)x).ToArray();
      var hex = Convert.ToHexString(arr);

      hasid = hashids.EncodeHex(hex);
    }
    return hasid;
  }
  // create contact with email and phone number and return contact

}


