
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.StandardNonFungibleTokenERC721;
using Nethereum.StandardNonFungibleTokenERC721.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace NethereumERC721
{

  class Program
  {
    static async Task Main(string[] _)
    {
      // await DeployERC721Async();
      // await MintingERC721Async();
      // await BalanceAndOwnerERC721Async();
      await BalanceAndOwnerUseFunctionERC721Async();
      // await TransferFromERC721Async();
      await TransferFromWithFunctionERC721Async();
      await BalanceAndOwnerUseFunctionERC721Async();
    }

    [Function("SafeTransferFrom")]
    public class SafeTransferFromFunction : FunctionMessage
    {
      [Parameter("address", "from", 1)]
      public string From { get; set; }

      [Parameter("address", "to", 2)]
      public string To { get; set; }

      [Parameter("uint256", "tokenId", 3)]
      public BigInteger TokenId { get; set; }
    }

    static async Task TransferFromWithFunctionERC721Async()
    {
      var privateKey = "0xffbb4ce6740cde20e66ed680e0a1fe1a2cbc1f749087623a3fd9fc1d6eee6eab";

      var account = new Account(privateKey);

      var web3 = new Web3(account, "http://localhost:8545");

      var safeTransferFromMessage = new SafeTransferFromFunction()
      {
        From = "0x8F71b7993F64D22DdBeE4c10Bb138d4039530237",
        To = "0xa20D2cFAb209C9082DD7b19e7743F7294059A10A",
        TokenId = 1
      };
      var contractHandler = web3.Eth.GetContractHandler("contractAddress ..");

      var result = await contractHandler.SendRequestAsync(safeTransferFromMessage).ConfigureAwait(false);

      Console.WriteLine($"result: {result}");
    }

    static async Task TransferFromERC721Async()
    {
      var privateKey = "0xffbb4ce6740cde20e66ed680e0a1fe1a2cbc1f749087623a3fd9fc1d6eee6eab";

      var account = new Account(privateKey);

      var web3 = new Web3(account, "http://localhost:8545");

      var tokenService = new ERC721Service(web3, "0x...");
      var result = await tokenService.TransferFromRequestAndWaitForReceiptAsync(
       "0x8F71b7993F64D22DdBeE4c10Bb138d4039530237",
       "0xa20D2cFAb209C9082DD7b19e7743F7294059A10A",
       1,
       new CancellationTokenSource());


      Console.WriteLine($"result: {result}");
    }

    [Function("ownerOf", "address")]
    public class OwnerOfFunction : FunctionMessage
    {
      [Parameter("uint256", "tokenId", 1)]
      public string TokenId { get; set; }
    }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
      [Parameter("address", "owner", 1)]
      public string Owner { get; set; }
    }
    static async Task BalanceAndOwnerUseFunctionERC721Async()
    {
      var privateKey = "0xffbb4ce6740cde20e66ed680e0a1fe1a2cbc1f749087623a3fd9fc1d6eee6eab";

      var account = new Account(privateKey);

      var web3 = new Web3(account, "http://localhost:8545");

      var ownerOfMessage = new OwnerOfFunction() { TokenId = 1 };
      var queryHandler = web3.Eth.GetContractQueryHandler<OwnerOfFunction>();

      var owner = await queryHandler.QueryAsync<string>("0x...", ownerOfMessage).ConfigureAwait(false);

      var balanceOfMessage = new BalanceOfFunction() { Owner = owner };
      var balanceQueryHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
      var balance = await balanceQueryHandler.QueryAsync<BigInteger>("0x...", balanceOfMessage).ConfigureAwait(false);


      Console.WriteLine($"owner: {owner}");
      Console.WriteLine($"balance: {balance}");
    }


    static async Task BalanceAndOwnerERC721Async()
    {
      var privateKey = "0xffbb4ce6740cde20e66ed680e0a1fe1a2cbc1f749087623a3fd9fc1d6eee6eab";

      var account = new Account(privateKey);

      var web3 = new Web3(account, "http://localhost:8545");

      var tokenService = new ERC721Service(web3, "0x...");

      var owner = await tokenService.OwnerOfQueryAsync(1);
      var balance = await tokenService.BalanceOfQueryAsync(owner);


      Console.WriteLine($"owner: {owner}");
      Console.WriteLine($"balance: {balance}");
    }

    static async Task DeployERC721Async()
    {
      var privateKey = "0xffbb4ce6740cde20e66ed680e0a1fe1a2cbc1f749087623a3fd9fc1d6eee6eab";

      var account = new Account(privateKey);

      var web3 = new Web3(account, "http://localhost:8545");

      var deploymentContract = new ERC721Deployment();

      var token = await ERC721Service.DeployContractAndWaitForReceiptAsync(web3, deploymentContract);

      Console.WriteLine($"Contract Address: {token.ContractAddress}");

    }

    [Function("safeMint")]
    public class SafeMintFunction : FunctionMessage
    {
      [Parameter("address", "to", 1)]
      public string To { get; set; }
      [Parameter("string", "uri", 2)]
      public string Uri { get; set; }
    }

    static async Task MintingERC721Async()
    {
      var privateKey = "0xffbb4ce6740cde20e66ed680e0a1fe1a2cbc1f749087623a3fd9fc1d6eee6eab";

      var account = new Account(privateKey);

      var web3 = new Web3(account, "http://localhost:8545");

      var safeMintFunc = new SafeMintFunction()
      {
        To = account.Address,
        Uri = "ipfs://Q......"
      };

      var contractHandler = web3.Eth.GetContractHandler("0x...");

      var result = await contractHandler.SendRequestAsync(safeMintFunc);

      Console.WriteLine($"result: {result}");

    }
  }
}
