using System.Numerics;
using System.Text;
using NBitcoin;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.HdWallet;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace NethereumSample
{
  class Program
  {
    static string _byteCode = "608060405234801561001057600080fd5b5061040b806100206000396000f3fe608060405234801561001057600080fd5b506004361061004c5760003560e01c806301243fce146100515780633eb76b9c1461006f5780635b57014c1461008b578063aec2ccae146100a9575b600080fd5b6100596100d9565b6040516100669190610224565b60405180910390f35b61008960048036038101906100849190610270565b6100df565b005b6100936101e5565b6040516100a09190610224565b60405180910390f35b6100c360048036038101906100be91906102fb565b6101eb565b6040516100d09190610343565b60405180910390f35b60005481565b600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff16158015610144575060018114806101435750600281145b5b61014d57600080fd5b60018103610171576000808154809291906101679061038d565b919050555061018a565b600160008154809291906101849061038d565b91905055505b6001600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060006101000a81548160ff02191690831515021790555050565b60015481565b60026020528060005260406000206000915054906101000a900460ff1681565b6000819050919050565b61021e8161020b565b82525050565b60006020820190506102396000830184610215565b92915050565b600080fd5b61024d8161020b565b811461025857600080fd5b50565b60008135905061026a81610244565b92915050565b6000602082840312156102865761028561023f565b5b60006102948482850161025b565b91505092915050565b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b60006102c88261029d565b9050919050565b6102d8816102bd565b81146102e357600080fd5b50565b6000813590506102f5816102cf565b92915050565b6000602082840312156103115761031061023f565b5b600061031f848285016102e6565b91505092915050565b60008115159050919050565b61033d81610328565b82525050565b60006020820190506103586000830184610334565b92915050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601160045260246000fd5b60006103988261020b565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff82036103ca576103c961035e565b5b60018201905091905056fea264697066735822122066dc95fb8fe3e67844bf6258adde053b5ca22f3e0b6adada51e9b31d3847060764736f6c63430008120033";
    static string _abi = "[{'inputs':[],'name':'candidate1','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'candidate2','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'uint256','name':'candidate','type':'uint256'}],'name':'castVote','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'','type':'address'}],'name':'voted','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'}]";
    static async Task Main(string[] args)
    {
      // ImportKeyFromKeyStore();
      // await TransferEther();
      // await DeploySmartContractAsync();
      //await GenerateHdWalletAsync();
      //await CallSmartContractAsync();
      //await EstimateGasAsync();
      //await GetNonceAsync();
      SignMsg();
    }

    static void SignMsg()
    {
      var message = "This is Nethereum curse";
      var privateKey = "0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b";
      var provideAddress = "0x569f5e5C9827588BDf39FA92FE482e08781e4482";

      var signer = new Nethereum.Signer.MessageSigner();

      var signature = signer.HashAndSign(message, privateKey);

      Console.WriteLine($"Signature: {signature}.");

      var address = signer.HashAndEcRecover(message, signature);
      Console.WriteLine($"Address: {address}.");
      Console.WriteLine(address = provideAddress);

    }

    static async Task GetNonceAsync()
    {

      var account = new Account("0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b");
      var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

      account.NonceService = new Nethereum.RPC.NonceServices.InMemoryNonceService(account.Address, web3.Client);

      var currenceNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(account.Address, Nethereum.RPC.Eth.DTOs.BlockParameter.CreatePending());

      Console.WriteLine($"Current nonce: {currenceNonce.Value}");
    }

    [Function("castVote")]
    public class VotingFunc : FunctionMessage
    {
      [Parameter("uint256", "candidate", 1)]
      public BigInteger Candidate { get; set; }

    }


    static async Task EstimateGasAsync()
    {
      string contractAddress = "0xCdA10cd7104814F2a327F4EE6C8014926060f152";
      var account = new Account("0x0da67fe1cac9c624206729a3d4a126e6e521d48b66fb1c84cb85d694566d3f04");
      var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

      var votingHandler = web3.Eth.GetContractTransactionHandler<VotingFunc>();

      var candidate = new VotingFunc()
      {
        Candidate = 2
      };

      var estimateGas = await votingHandler.EstimateGasAsync(contractAddress, candidate);

      Console.WriteLine($"Estimated gas: {estimateGas.Value}");
    }





    static async Task CallSmartContractAsync()
    {
      string contractAddress = "0xCdA10cd7104814F2a327F4EE6C8014926060f152";
      var account = new Account("0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b");
      var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

      Contract voteContract = web3.Eth.GetContract(_abi, contractAddress);
      var candidate1 = await voteContract.GetFunction("candidate1").CallAsync<BigInteger>();
      var candidate2 = await voteContract.GetFunction("candidate2").CallAsync<BigInteger>();
      Console.WriteLine($"Candidate1 has: {candidate1} votes.");
      Console.WriteLine($"Candidate2 has: {candidate2} votes.");

      var gasPrice = new HexBigInteger(400000);
      var value = new HexBigInteger(0);

      var castVoteResult = await voteContract.GetFunction("castVote").SendTransactionAsync(account.Address, gasPrice, value, 1);

      candidate1 = await voteContract.GetFunction("candidate1").CallAsync<BigInteger>();
      candidate2 = await voteContract.GetFunction("candidate2").CallAsync<BigInteger>();
      Console.WriteLine($"Candidate1 has: {candidate1} votes.");
      Console.WriteLine($"Candidate2 has: {candidate2} votes.");
    }

    static async Task DeploySmartContractAsync()
    {
      var account = new Account("0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b");
      var web3 = new Web3(account, "HTTP://127.0.0.1:8545");

      var gasPrice = new Nethereum.Hex.HexTypes.HexBigInteger(8000000);
      var txHash = await web3.Eth.DeployContract.SendRequestAsync(_abi, _byteCode, account.Address, gasPrice);
      Console.WriteLine($"TxHash: {txHash}");
    }

    static async Task GenerateHdWalletAsync()
    {
      var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
      Console.WriteLine($"The 12 seed words are: {mnemonic}");

      var password = "[{'inputs':[],'name':'candidate1','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'candidate2','outputs':[{'internalType':'uint256','name':'','type':'uint256'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'uint256','name':'candidate','type':'uint256'}],'name':'castVote','outputs':[],'stateMutability':'nonpayable','type':'function'},{'inputs':[{'internalType':'address','name':'','type':'address'}],'name':'voted','outputs':[{'internalType':'bool','name':'','type':'bool'}],'stateMutability':'view','type':'function'}]";
      var wallet = new Wallet(mnemonic.ToString(), password);

      for (int i = 0; i < 10; i++)
      {
        var account = wallet.GetAccount(i);
        Console.WriteLine($"Address at index {i}: {account.Address} and private key: {account.PrivateKey}");
      }

    }

    static async Task UnitConversationAsync()
    {

      var web3 = new Web3("https://mainnet.infura.io/v3/62404da4dad441959c578aa340a15662");
      var balance = await web3.Eth.GetBalance.SendRequestAsync("0x95222290DD7278Aa3Ddd389Cc1E1d165CC4BAfe5");
      Console.WriteLine($"Balance of account: {balance.Value}");

      var balanceInEther = Web3.Convert.FromWei(balance.Value);
      Console.WriteLine($"Balance of account in Ether: {balanceInEther}");


      var balanceInWei = Web3.Convert.ToWei(balance.Value);
      Console.WriteLine($"Balance of account in wei: {balanceInWei}");


    }

    static async Task TransferEtherAsync()
    {
      var privateKey = "0xe513171ac1754239c5af447375d887647ab650b07efc0a476d6176aea017bd1b";
      var account = new Account(privateKey);

      var web3 = new Web3(account, "HTTP://127.0.0.1:8545");
      var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
      Console.WriteLine($"Balance before transaction: {balance.Value}");

      var tx = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync("0x4Af3911cfb54784095214a9f305040A724bC6927", 1);

      balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
      Console.WriteLine($"Balance after transaction: {balance.Value}");

    }

    static void ImportKeyFromKeyStore()
    {
      string jsonContent = @"{""""}";
      string password = "";
      var account = Account.LoadFromKeyStore(jsonContent, password);
      Console.WriteLine($"Address: {account.Address}.");

    }

    static void ImportKey()
    {
      var privateKey = "0xb3848061e7f69b6941fceb9d07cb37eff1ddaf39c4f6740a2a869e67a29efb19";
      var account = new Account(privateKey);
      Console.WriteLine($"Address: {account.Address}.");

    }

    static void Key()
    {
      var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
      var privateKey = ecKey.GetPrivateKey();
      Console.WriteLine($"Private Key: {privateKey}.");

      var publicKeyByte = ecKey.GetPubKey();
      var publicKey = ByteArrayToString(publicKeyByte);
      Console.WriteLine($"Public Key: {publicKey}.");

      var address = ecKey.GetPublicAddress();
      Console.WriteLine($"Address: {address}.");

    }

    static string ByteArrayToString(byte[] ba)
    {
      StringBuilder sb = new(ba.Length * 2);
      foreach (var b in ba)
      {
        sb.AppendFormat("{0:x2}", b);

      }
      return sb.ToString();

    }
    static async Task GetAccountBalanceAsync()
    {
      var web3 = new Web3("https://mainnet.infura.io/v3/62404da4dad441959c578aa340a15662");
      var balance = await web3.Eth.GetBalance.SendRequestAsync("0x95222290DD7278Aa3Ddd389Cc1E1d165CC4BAfe5");
      Console.WriteLine($"Balance in  Wei: {balance.Value}");
    }


  }
}