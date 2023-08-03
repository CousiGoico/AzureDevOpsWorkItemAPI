public class Program {

    static void Main(string[] args) {
        Console.WriteLine("Introduzca organización:");
        string organization = Console.ReadLine();
        Console.WriteLine("Introduzca proyecto:");
        string project = Console.ReadLine();
        Console.WriteLine("Introduzca id del workitem:");
        string idWorkItem = Console.ReadLine();
        Console.WriteLine("Introduzca PAT:");
        string pat = Console.ReadLine();

        string result = CallAzureDevOpsAPI(organization, project, idWorkItem, ConvertBytesBase64(pat)).Result;

        Console.WriteLine($"Resultado: {result}");
    }

    private static string ConvertBytesBase64(string PAT){
        return Convert.ToBase64String(
                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", PAT)));
    }

    private static async Task<string> CallAzureDevOpsAPI(string organization, string project, string idWorkItem, string bytesBase64PAT) {

        var client = new HttpClient();
        var url = string.Format("https://dev.azure.com/{0}/{1}/_apis/wit/workitems?ids={2}&$expand=all&api-version=7.0", organization, project, idWorkItem);
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"basic {bytesBase64PAT}");
        client.DefaultRequestHeaders.Add("Authorization", $"basic {bytesBase64PAT}");
        Task<HttpResponseMessage> response = client.SendAsync(request);
        response.Result.EnsureSuccessStatusCode();
        return response.Result.Content.ReadAsStringAsync().Result;
    }
}