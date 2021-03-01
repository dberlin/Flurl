using System;
using System.Globalization;
using System.Text;
using Crestron.SimplSharp.CrestronIO;
using Newtonsoft.Json;

namespace Flurl.Http.Configuration
{
	/// <summary>
	/// ISerializer implementation that uses Newtonsoft Json.NET.
	/// Default serializer used in calls to GetJsonAsync, PostJsonAsync, etc.
	/// </summary>
	public class NewtonsoftJsonSerializer : ISerializer
	{
		private readonly JsonSerializerSettings _settings;

		/// <summary>
		/// Initializes a new instance of the <see cref="NewtonsoftJsonSerializer"/> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		public NewtonsoftJsonSerializer(JsonSerializerSettings settings) {
			_settings = settings;
		}


		private static string SerializeObjectInternal(object value, JsonSerializer jsonSerializer) {
			StringBuilder sb = new StringBuilder(256);
			StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonWriter = new JsonTextWriter(sw)) {
				jsonSerializer.Serialize(jsonWriter, value);
			}

			return sw.ToString();
		}
		/// <summary>
		/// Serializes the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		public string Serialize(object obj) {
			JsonSerializer serializer = JsonSerializer.Create(_settings);
			return SerializeObjectInternal(obj,  serializer);
		}

		/// <summary>
		/// Deserializes the specified s.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		public T Deserialize<T>(string s) {
			return JsonConvert.DeserializeObject<T>(s, _settings);
		}

		/// <summary>
		/// Deserializes the specified stream.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		public T Deserialize<T>(Stream stream) {
			// https://www.newtonsoft.com/json/help/html/Performance.htm#MemoryUsage
			using (var sr = new StreamReader(stream))
			using (var jr = new JsonTextReader(sr)) {
				return JsonSerializer.Create(_settings).Deserialize<T>(jr);
			}
		}
	}
}