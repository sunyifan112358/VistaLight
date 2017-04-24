using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class MapSerializer {

	public void SaveMap(Map map, string path) {
		BinaryFormatter serializer = new BinaryFormatter();
		FileStream file = File.Create(path);

		serializer.Serialize(file, map);
		file.Close();
	}

	public Map LoadMap(string mapfile) {
        //Change reference to path of file to Resources.Load for file.
		Map map = null;
		try {
            TextAsset ta = (TextAsset) Resources.Load(mapfile);
            Stream file = new MemoryStream(ta.bytes);
			BinaryFormatter deserializer = new BinaryFormatter();
			map = (Map)deserializer.Deserialize(file);
			RecoverReferenceInNodes(map);
			file.Close();
		} catch (FileNotFoundException e) {
			Debug.Log(e.Message);
		} catch (IOException e) {
			Debug.Log(e.Message);
		}
		return map;
	}

	private void RecoverReferenceInNodes(Map map) {
		foreach (Node node in map.nodes) {
			foreach (Connection connection in map.connections) {
				if (connection.StartNode == node || connection.EndNode == node) {
					node.AddConnection(connection);
				}
			}
		}
		
	}
}
