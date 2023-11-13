using Nethereum.Web3;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.Blocks;
using Nethereum.Hex.HexTypes;

namespace ChainInfoSample
{
  class Program
  {

    static async Task Main(string[] args)
    {
      // var web3 = new Web3("https://mainnet.infura.io/v3/62404da4dad441959c578aa340a15662");
      var web3 = new Web3("HTTP://127.0.0.1:8545");
      var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
      Console.WriteLine($"The current BlockNumber is: {blockNumber.Value}");

      var balance = await web3.Eth.GetBalance.SendRequestAsync("0x2a039264E5C290A530AA38720A3c0e6A22ed0be7");
      Console.WriteLine($"Balance in Wei: {balance.Value}");


      var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(blockNumber.Value));
      Console.WriteLine($"The current BlockNumber is: {block.Number.Value}");
      Console.WriteLine($"The current Block Hash is: {block.BlockHash}");
      Console.WriteLine($"The no of Transaction: {block.Transactions.Length}");

      var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync("0xa9174a6d360e1cd503b27ffeaa35009aaebc43359c3f59b78a5c66a8f3916bc7");
      Console.WriteLine($"Transaction From: {transaction.From}");
      Console.WriteLine($"Transaction To: {transaction.To}");
      Console.WriteLine($"Transaction Value: {transaction.Value}");

      var transactionReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync("0xa9174a6d360e1cd503b27ffeaa35009aaebc43359c3f59b78a5c66a8f3916bc7");
      Console.WriteLine($"Transaction Logs: {transactionReceipt.Logs.Count}");
      Console.WriteLine($"Transaction Hash: {transactionReceipt.TransactionHash}");
    }



  }
}