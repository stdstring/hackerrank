// from https://www.hackerrank.com/challenges/encryption

using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using ProblemSolving.Common;

namespace Algorithms.Implementation
{
    public class EncryptionTask : ITask
    {

        public EncryptionTask(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
        }

        public Int32 Execute(String[] args)
        {
            String input = _inputReader.ReadLine();
            _outputWriter.WriteLine(Encrypt(input));
            return 0;
        }

        private String Encrypt(String input)
        {
            String source = input.Replace(" ", "");
            Int32 rowCount = (Int32)Math.Sqrt(source.Length);
            Int32 columnCount = rowCount;
            if (rowCount * columnCount < source.Length)
                ++columnCount;
            if (rowCount * columnCount < source.Length)
                ++rowCount;
            StringBuilder builder = new StringBuilder();
            for (Int32 column = 0; column < columnCount; ++column)
            {
                for (Int32 row = 0; row < rowCount; ++row)
                {
                    Int32 index = row * columnCount + column;
                    if (index >= source.Length)
                        break;
                    builder.Append(source[index]);
                }
                builder.Append(' ');
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        private readonly TextReader _inputReader;
        private readonly TextWriter _outputWriter;
    }

    [TestFixture]
    public class EncryptionTests
    {
        [TestCase("haveaniceday", "hae and via ecy")]
        [TestCase("feedthedog", "fto ehg ee dd")]
        [TestCase("chillout", "clu hlt io")]
        [TestCase("ifmanwasmeanttostayonthegroundgodwouldhavegivenusroots",
                  "imtgdvs fearwer mayoogo anouuio ntnnlvt wttddes aohghn sseoau")]
        [TestCase("iffactsdontfittotheorychangethefacts", "isieae fdtonf fotrga anoyec cttctt tfhhhs")]
        [TestCase("hxfcbvrqssbjavupuhby", "hvbp xrju fqah csvb bsuy")]
        [TestCase("roqfqeylxuyxjfyqterizzkhgvngapvudnztsxeprfp", "rlyzatp oxqkps quthvx fyegue qxrvdp ejinnr yfzgzf")]
        [TestCase("wclwfoznbmyycxvaxagjhtexdkwjqhlojykopldsxesbbnezqmixfpujbssrbfhlgubvfhpfliimvmnny",
                  "wmgjpnull cyjqlejgi lyhhdzbui wctlsqsbm fxeoxmsvv ovxjeirfm zadysxbhn nxkkbffpn bawobphfy")]
        [TestCase("a", "a")]
        [TestCase("iuo", "io u")]
        [TestCase("jqlvizzusfkmuevvtcbnlcvmocjvfskqkyluxmqru", "juvcfu qsvvsx lftmkm vkcoqq imbckr zunjyu zelvl")]
        [TestCase("bgwdyygtmwhtwhusfedckrgybozfjaukgsngqvzftiypqukxypbkghjiwkphkjtsdizueaz",
                  "bwdfqujs ghcjvkid wtkazxwi dwrufykz yhgktppu yuygibhe gsbsykka tfonpgjz mezgqht")]
        public void Execute(String input, String expectedOutput)
        {
            TaskExecutor.Execute((inputReader, outputWriter) => new EncryptionTask(inputReader, outputWriter), input, expectedOutput);
        }
    }
}
