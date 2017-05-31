using SimpleJSON;
using System.Collections;
using System.IO;
using UnityEngine;

// Adapter for logging to a file.
public class FileAdapter : BaseAdapter
{
	public string logfilePrefix;

	private StreamWriter m_out = null;
	private bool m_inited = false; // has init been called?
	private string m_error = null; // a string describing any error

	// Try to initialize logging to the given file.  If logging has already been initialized, do nothing.
	public
	override
	void
	Init()
	{
		if (m_inited) {
			Debug.Log("Re-initializing logger");
			return;
		}

		m_inited = true;

		try {
			m_out = new StreamWriter(logfilePrefix + "-" + System.Guid.NewGuid().ToString() + ".json");
			
			m_out.Write("# version:1\n");
			m_out.Flush();
		} catch (System.Exception ex) {
			m_error = ex.Message;
			Debug.Log(m_error);
		}
	}
	
	// Finish file logging.
	public
	override
	void
	Fini()
	{
		if (m_error != null) {
			return;
		}
		if (!m_inited) {
			Debug.Log("Finalizing uninitialized logger");
			return;
		}

		try {
			m_out.Flush();
			m_out.Close();
			m_out = null;
		} catch (System.Exception ex) {
			m_error = ex.Message;
			Debug.Log(m_error);
		}
	}
	/*
	public
	override
	void
	Handle(JSONClass node)
	{
		if (m_error != null) {
			return;
		}
		if (!m_inited) {
			Debug.Log("Logger not initialized");
			return;
		}
		
		try {
			string node_output = node.ToString().Trim().Replace("\n", " ") + "\n";
			
			m_out.Write(node_output);
			m_out.Flush();
		} catch (System.Exception ex) {
			m_error = ex.Message;
			Debug.Log(m_error);
		}
	}
    */
    public
    override
    void
    Handle(JSONClass node)
    {
        if (m_error != null)
        {
            return;
        }
        if (!m_inited)
        {
            Debug.Log("Logger not initialized");
            return;
        }
        try
        {
            string node_output = "[{data:true}]";// node.ToString().Trim().Replace("\n", " ") + "\n";
            m_out.Write(node_output);
            m_out.Flush();
            string urlBase = "http://107.21.26.163/secphp/json_to_server.php?user=nugs&pass=7dc2110243bfbd86f83bbeb4d412e1ce";
            Debug.Log(urlBase);
            WWW data;
            // string node_output = WWW.EscapeURL(node.ToString().Trim());
            string url = urlBase + "&json=" + node_output + "&file=optimization/" + node["data"]["session_id"] + ".json";
            print(url);
            WWWForm form = new WWWForm();
            form.AddField("data", node_output);
            data = new WWW(url, form);
            Debug.Log("Sending data to: " + url);
            Debug.Log("node_output " + node_output);

            if (DoWWW(data) != null)
            {
                StartCoroutine(DoWWW(data));
            }
        }
        catch (System.Exception ex)
        {
            m_error = ex.Message;
            Debug.Log(m_error);
        }



    }

    IEnumerator DoWWW(WWW www)
    {
        yield return www;
        //Debug.Log("Web returned: " + www.text);
        if (www.error != null)
        {
            Debug.Log("Web error: " + www.error);
        }
    }

    // If there has been any error in file handling, return a description of that error, otherwise, null.
    public
	override
	string
	Error()
	{
		return m_error;
	}
}
