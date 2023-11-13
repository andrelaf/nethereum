
using Nethereum.RPC.Eth.DTOs;
using Nethereum.StandardTokenEIP20;
using Nethereum.StandardTokenEIP20.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace NethereumERC20
{
  class Program
  {
    static async Task Main(string[] _args)
    {
      //await DeployErc20TokenAsync();
      //await Ecr20InfoAsync();
      //await TransferTokenAsync();
      await TransferFromDemo();
    }


    static async Task TransferFromDemo()
    {
      var contractAddress = "0x2e01ca3b85e8913e197698ca954e091aad7ecdd8";
      var accountFrom = new Account("0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b");
      var web3 = new Web3(accountFrom, "http://localhost:8545");

      var tokenSrv = new StandardTokenService(web3, contractAddress);
      var balanceAccountFrom = await tokenSrv.BalanceOfQueryAsync(accountFrom.Address);

      Console.WriteLine($"Balance Account From: {balanceAccountFrom}");

      var account1Address = "0x569f5e5C9827588BDf39FA92FE482e08781e4482";
      var account2Address = "0x4Af3911cfb54784095214a9f305040A724bC6927";
      var account3Address = "0x07f91ce8F529648b9D867d41e3eCc1c14eAdc9F2";

      var aproveTransferReceipt = await tokenSrv.ApproveRequestAndWaitForReceiptAsync(account3Address, 50000);
      Console.WriteLine("aproveTransferReceipt TXID: " + aproveTransferReceipt.TransactionHash);

      var allowanceAmount = await tokenSrv.AllowanceQueryAsync(account1Address, account3Address);
      Console.WriteLine("allowanceAmount: " + allowanceAmount);

      var account3 = new Account("0xc42f5002672132eb7c5eff4dc373ebd99bf520136747cec59f5e22d011c04907");
      var web3From = new Web3(account3, "http://localhost:8545");

      var tokenFromSrv = new StandardTokenService(web3From, contractAddress);

      var transferFromReceipt = await tokenFromSrv.TransferFromRequestAndWaitForReceiptAsync(account1Address, account2Address, 23000);
      Console.WriteLine("transferFromReceipt: " + transferFromReceipt.TransactionHash);


      Console.WriteLine("<---------------- transferFrom --------------------->");

      var balanceAccount1 = await tokenFromSrv.BalanceOfQueryAsync(account1Address);
      Console.WriteLine($"Balance Account 1: {balanceAccount1}");

      var balanceAccount2 = await tokenFromSrv.BalanceOfQueryAsync(account2Address);
      Console.WriteLine($"Balance Account 2: {balanceAccount2}");

      var balanceAccount3 = await tokenFromSrv.BalanceOfQueryAsync(account3Address);
      Console.WriteLine($"Balance Account 3: {balanceAccount3}");
    }

    static async Task TransferTokenAsync()
    {
      var contractAddress = "0x2e01ca3b85e8913e197698ca954e091aad7ecdd8";
      var accountFrom = new Account("0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b");
      var web3 = new Web3(accountFrom, "http://localhost:8545");

      var tokenSrv = new StandardTokenService(web3, contractAddress);

      var balanceAccountFrom = await tokenSrv.BalanceOfQueryAsync(accountFrom.Address);

      Console.WriteLine($"Balance Account From: {balanceAccountFrom}");

      var accountTo = "0x4Af3911cfb54784095214a9f305040A724bC6927";

      var transferReceipt = await tokenSrv.TransferRequestAndWaitForReceiptAsync(accountTo, 1230000);

      balanceAccountFrom = await tokenSrv.BalanceOfQueryAsync("0x569f5e5C9827588BDf39FA92FE482e08781e4482");
      Console.WriteLine($"Balance Account From: {balanceAccountFrom}");

      var balanceAccountTo = await tokenSrv.BalanceOfQueryAsync(accountTo);
      Console.WriteLine($"Balance Account To: {balanceAccountTo}");


      var transferEvent = tokenSrv.GetTransferEvent();

      var allTransferFilter = await transferEvent.CreateFilterAsync(new BlockParameter(transferReceipt.BlockNumber));

      var eventsLogAll = await transferEvent.GetAllChangesAsync(allTransferFilter);

      var transferLog = eventsLogAll.First();

      Console.WriteLine($"Transfer Hash: {transferLog.Log.TransactionHash}");
      Console.WriteLine($"Block Number: {transferLog.Log.BlockNumber.Value}");
      Console.WriteLine($"Event To: {transferLog.Event.To}");
      Console.WriteLine($"Event From: {transferLog.Event.From}");
      Console.WriteLine($"Value: {transferLog.Event.Value}");
    }

    static async Task Ecr20InfoAsync()
    {
      var contractAddress = "0x2e01ca3b85e8913e197698ca954e091aad7ecdd8";
      var account = new Account("0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b");
      var web3 = new Web3(account, "http://localhost:8545");

      var tokenSrv = new StandardTokenService(web3, contractAddress);

      var totalSupply = await tokenSrv.TotalSupplyQueryAsync();
      Console.WriteLine($"Total Supply: {totalSupply}");

      var tokenName = await tokenSrv.NameQueryAsync();
      Console.WriteLine($"Token Name: {tokenName}");

      var symbol = await tokenSrv.SymbolQueryAsync();
      Console.WriteLine($"Token Symbol: {symbol}");


    }

    static async Task DeployErc20TokenAsync()
    {
      var account = new Account("0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b");
      var web3 = new Web3(account, "http://localhost:8545");

      ulong totalSupply = 20000000000;
      var deploymentContract = new EIP20Deployment()
      {
        InitialAmount = totalSupply,
        TokenName = "TestToken",
        TokenSymbol = "TT"
      };

      var tokenService = await StandardTokenService.DeployContractAndWaitForReceiptAsync(web3, deploymentContract);

      Console.WriteLine($"Contract Address: {tokenService.ContractAddress}");
    }
  }


}
