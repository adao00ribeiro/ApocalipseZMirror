using DarkRift;

public struct LoginRequestData : INetworkingData
{
	public string Email;
    public string Senha;
	public LoginRequestData(string email,string senha)
	{
		Email = email;
        Senha = senha;
	}

    public void Deserialize(DeserializeEvent e)
    {
        Email = e.Reader.ReadString();
        Senha = e.Reader.ReadString();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Email);
        e.Writer.Write(Senha);
    }
}
