using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using OpenTokSDK;

namespace ArchiveDecryption
{
	class MainClass
	{
		private static void showHelp ()
		{
			Console.WriteLine ("Archive Encryption/Decryption Demo tool");
			Console.WriteLine ("Arguments: [-e|-d] -i <input> -c <X509Certificate> -p <base64Password> -o <outfile>");
		}			
		public static void Main (string[] args)
		{
			string certFile = null;
			string inputFile = null;
			string outputFile = null;
			string password = null;
			byte[] input;
			byte[] output;
			bool encrypt = false;
			bool decrypt = false;
			for (int i = 0; i < args.Length; i++) {
				if (args [i] == "-h") {
					showHelp ();
				} else
				if (args[i] == "-i") {
					inputFile = args[++i];
				} else if (args[i] == "-o") {
					outputFile = args[++i];
				} else if (args[i] == "-d") {
					decrypt = true;
				} else if (args[i] == "-e") {
					encrypt = true;
				} else if (args[i] == "-p") {
					password = args[++i];
				} else if (args[i] == "-c") {
					certFile = args[++i];
				}
			}
				
			try {
				if (encrypt) {
					if (inputFile == null || certFile == null) {
						Console.WriteLine ("input file and certificate are mandatory for encryption");
						return;
					}
					input = File.ReadAllBytes(inputFile);
					X509Certificate2 certificate = new X509Certificate2(X509Certificate.CreateFromCertFile(certFile));
					if (certificate.PublicKey == null) {
						Console.WriteLine ("the provided certificate doesn't have a public key");
						return;
					}

					ArchiveCryptoUtils.EncryptBlob(certificate, input, out password, out output);
					File.WriteAllBytes(outputFile, output);
					Console.Write ("Password={0}", password);
				} else if (decrypt) {
					if (inputFile == null || certFile == null) {
						Console.WriteLine ("input file, password and certificate are mandatory for decryption");
						return;
					}
					input = File.ReadAllBytes(inputFile);
					// We expect the input to be a PFX file with the private key
					X509KeyStorageFlags flags = X509KeyStorageFlags.Exportable;
					Console.Write("Enter Certificate Password: ");
					string certPass = Console.ReadLine();
					byte[] certData = File.ReadAllBytes(certFile);
					X509Certificate2 certificate = new X509Certificate2(certData, certPass, flags);
					if (certificate.PrivateKey == null) {
						Console.WriteLine ("the provided certificate doesn't have a private key");
						return;
					}
					ArchiveCryptoUtils.DecryptBlob(certificate, password, input, out output);
					File.WriteAllBytes(outputFile, output);
				} else {
					Console.WriteLine("Either encrypt (-e) or decrypt (-d) must be specified");
				}
			} catch(Exception e) {
				Console.WriteLine(e.ToString());
			}
		}
	}
}
