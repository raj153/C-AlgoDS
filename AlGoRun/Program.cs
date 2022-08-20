using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlGoLiv.ProbSol.DS;
using AlGoLiv.ProbSol.String;
using System.Xml;
using Newtonsoft;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using AlGoLiv.ProbSol.Tree;
using AlGoLiv.ProbSol.Graph;
using AlGoLiv.ProbSol.Arrays;
using AlGoLiv.ProbSol.Logic;
using AlGoLiv.OOP;
using System.Threading;
using AlGoLiv.ProbSol.DoublyLinkedList;


namespace AlGoRun
{
    public class CampaignMemberStatus
    {
        public int Id;
        public string ExternalId;
        public string InternalName;
        public string DisplayName;
        public int ExternalCampaignId;

    }
    class Program
    {
        public delegate void M(string s);

        static void Main(string[] args)
        {
            Misc.TwoCityScheduleMinimizeCostUsingHashMap(new int[,] { 
                {20,60 },{10,40 },{40,200 },{30,300 } });
            MatrixOp.KClosestPointsToOrigin(new int [,]{ { 3, 3 },{5,-1 },{ -2, 4 } }, 2);

            var val2 = Misc.NumberWaysToDecode("11");

            //ListBasic.PartitionList<int>(null, 5, new IntComparer());
            MessageQueueTester.Run();
            var res= MatrixOp.NQueens(4);

            var comb= Backtracking.LetterCombinationsOfPhoneNum("3456");
            MatrixOp matrixOp = new MatrixOp();
            matrixOp.MaxPathSumGold(new int[,] { { 0, 6, 0 }, { 5, 8, 7 }, { 0, 9, 0 } });
            State state = new State(PrinterType.ODD);
            PrintOddEvenMultiThreadConcurrency oddPrinter = new PrintOddEvenMultiThreadConcurrency(
                2, 1, state, PrinterType.ODD, PrinterType.EVEN, 10);

            PrintOddEvenMultiThreadConcurrency evenPrinter = new PrintOddEvenMultiThreadConcurrency(
                2, 2, state, PrinterType.EVEN, PrinterType.ODD, 10);

            Thread oddThread = new Thread(oddPrinter.Run);
            Thread evenThread = new Thread(evenPrinter.Run);

            oddThread.Start();
            Thread.Sleep(100);
            evenThread.Start();

            oddThread.Join();
            evenThread.Join();
            var result1=Backtracking.CombinationSum(new int[]{ 2,3,6,7}, 7);
            PrintOddEvenTwoThreadImproved p1 = new PrintOddEvenTwoThreadImproved();

            p1.Print();
            return;
            PrintOddEvenTwoThread p = new PrintOddEvenTwoThread();
            p.Print();

            return;
            //return;
            Heap h = new Heap(new int[] { 3, 6, 8, 2, 1, 4, 9 });
            h.HeapSort();

            
            string[] words = { "cat", "dog", "byte", "tube", "can", "ant", "car", "tank" };

            Trie t1 = new Trie();
            t1.BuildTrie(words);

            Boggle boggle = new Boggle();
            boggle.PlayBoggle(t1);

            var judge = Misc.TownJudge(4, new int[,] { {1,3 }, { 1, 4 }, { 2,3}, { 2, 4 },{ 4, 3 } });
            //var res = Misc.RemoveAdjacentDuplicates("acaaabbacbbbnp");
            MatrixOp matOp = new MatrixOp();
            var image = new int[,] { { 1, 2, 1, 1 }, { 2, 1, 1, 2 }, { 1, 0, 1, 0 } };
            matOp.FloodFill(image, 1, 2, 3);

            TreeNode node = new TreeNode(3);
            node.Left = new TreeNode(5);
            node.Right = new TreeNode(1);
            node.Left.Left = new TreeNode(6);
            node.Left.Right = new TreeNode(2);
            node.Left.Right.Left = new TreeNode(7);
            node.Left.Right.Right = new TreeNode(4);
            node.Right.Left = new TreeNode(0);
            node.Right.Right = new TreeNode(8);

            var result =new DistanceFromTarget().DistanceKFromTargetT(node, node.Left, 2);

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            CampaignMemberStatus[] camps = new CampaignMemberStatus[3];
            camps[0] = new CampaignMemberStatus() { Id = 1, ExternalId = System.Guid.NewGuid().ToString(), InternalName = "sent", ExternalCampaignId = 1, DisplayName = "Sent" };
            camps[1] = new CampaignMemberStatus() { Id = 2, ExternalId = System.Guid.NewGuid().ToString(), InternalName = "responded", ExternalCampaignId = 1, DisplayName = "Responded" };
            camps[2] = new CampaignMemberStatus() { Id = 3, ExternalId = "", InternalName = "newstatus", ExternalCampaignId = 1, DisplayName = "New Status" };
           var cs= Newtonsoft.Json.JsonConvert.SerializeObject(camps, new JsonSerializerSettings() {
               ContractResolver = contractResolver,
               Formatting = Newtonsoft.Json.Formatting.Indented

           });


            var reader = new XmlTextReader(new System.IO.StringReader("<?xml version='1.0' encoding='utf - 8'?>  <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns='urn:partner.soap.sforce.com' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>   <soapenv:Body>    <loginResponse>     <result>      <metadataServerUrl>https://experian.my.salesforce.com/services/Soap/m/42.0/00Di0000000faSr</metadataServerUrl>      <passwordExpired>false</passwordExpired>      <sandbox>false</sandbox>      <serverUrl>https://experian.my.salesforce.com/services/Soap/u/42.0/00Di0000000faSr</serverUrl>      <sessionId>00Di0000000faSr!ARsAQAW7hG2jDgDQfoWV1mFRt8rRWXnuKFSNX2JHKWYlfs1KKSLyyjnXEqpfJZWoDHClaMtfpTQzCM_4tm2q.Nouzt3uZIoZ</sessionId>      <userId>005i0000001w5sYAAQ</userId>      <userInfo>       <accessibilityMode>false</accessibilityMode>       <chatterExternal>false</chatterExternal>       <currencySymbol xsi:nil='true' />       <orgAttachmentFileSizeLimit>5242880</orgAttachmentFileSizeLimit>       <orgDefaultCurrencyIsoCode xsi:nil='true' />       <orgDefaultCurrencyLocale xsi:nil='true' />       <orgDisallowHtmlAttachments>false</orgDisallowHtmlAttachments>       <orgHasPersonAccounts>false</orgHasPersonAccounts>       <organizationId>00Di0000000faSrEAI</organizationId>       <organizationMultiCurrency>true</organizationMultiCurrency>       <organizationName>Experian Services Corp.</organizationName>       <profileId>00ei0000000LJ7uAAG</profileId>       <roleId>00Ei0000000MviBEAS</roleId>       <sessionSecondsValid>14400</sessionSecondsValid>       <userDefaultCurrencyIsoCode>USD</userDefaultCurrencyIsoCode>       <userEmail>marketingautomation@experian.com</userEmail>       <userFullName>Marketing Automation</userFullName>       <userId>005i0000001w5sYAAQ</userId>       <userLanguage>en_US</userLanguage>       <userLocale>en_US</userLocale>       <userName>marketingautomation@experian.global</userName>       <userTimeZone>America/Los_Angeles</userTimeZone>       <userType>Standard</userType>       <userUiSkin>Theme3</userUiSkin>      </userInfo>     </result>    </loginResponse>   </soapenv:Body>  </soapenv:Envelope>"));
            reader.Read();
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\Eloqua\CRMUsage\ColoPlastResp.xml");
            XmlNamespaceManager xmanager = new XmlNamespaceManager(doc.NameTable);
            xmanager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            //xmanager.AddNamespace("x", "http://bank.co.com/Service/2011_01/Service");


            var nodes = doc.SelectNodes("soapenv:Envelope/soapenv:Body",xmanager);

             
            LRUCache<int, string> cache = new AlGoLiv.ProbSol.DS.LRUCache<int, string>(10);
            cache.Insert(10, "ABCD");
            try
            {
                throw new NullReferenceException("C");
                Console.Write("dfd");
            }catch(ArithmeticException a)
            {

            }
            M m = new M(s => { });
            string str = Console.ReadLine();
            Console.WriteLine($"First Max Len Even Word is {Parse.FindFirstMaxLenEvenWordFrmString(str)}");
            Console.ReadLine();

            //b2();
            a a1= new a();

             a1 = new b();

             a1 = new c();


             b b1 =  (b) a1;
        }

        private static void M2(string s)
        {
            throw new NotImplementedException();
        }

        void b2(M m)
        {

        }
        interface I
        {
            //public string Type { get; set; }

        } 
        class a
        {

        }

        class b : a
        {

        }
        class c:b
        {

        }
        public class Model
        {
             public event M M2;
             private void dd(string s)
             {
                 M2("Hello");
             }

        }

        public class view
        {
             view(Model m)
            {
              m.M2 += new M(dd);
              
            }

            private void dd(string s)
            {
                throw new NotImplementedException();
            }
        }

        }

    }
