//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.IO;
using System.Text;
using UnityEngine;


public class ByteArray
{

	public  const string ENCODING_cp1251 = "cp1251";

	public byte[] bytes;	
	public int pos;
	private MemoryStream stream;
	

	public ByteArray (byte[] array = null)
	{
		stream = new MemoryStream();
		if (array != null) {
			stream.Write(array, 0, array.Length);
			stream.Seek(0, SeekOrigin.Begin);
		}
	}

	public void writeInt(int val){
		stream.Write (BitConverter.GetBytes (val), 0, 4);
	}

	public void writeBigEndianInt(int val){
		byte[] i = BitConverter.GetBytes (val);
		byte[] n = new byte[4];
		n[3] = i[0];
		n[2] = i[1];
		n[1] = i[2];
		n[0] = i[3];
		stream.Write(n , 0, 4);
	}

	public void writeByte(byte val){
		stream.WriteByte(val);
	}

	public void writeBytes(byte[] val){
		stream.Write(val, 0, val.Length);
	}

	public void writeBytes(byte[] val, int len){
		stream.Write(val, 0, len);
	}

	public void writeString(string val,string encoding = "UTF-8" ){
		byte[] byte_string = null;
		switch (encoding) {
			case "UTF-8":{
			byte_string = System.Text.Encoding.UTF8.GetBytes(val);
				break;
			}
			case "cp1251":{
				byte_string = System.Text.Encoding.ASCII.GetBytes(val);
				break;
			}
		}
		writeInt (byte_string.Length);
		stream.Write(byte_string, 0 , byte_string.Length);
	}


	public int getInt(){
		byte[] b = new byte[4];
		stream.Read (b,0,4);
		return BitConverter.ToInt32(b, 0);
	}

    public int getIntAt(int pos) {
        byte[] b = new byte[4];
        stream.Read(b, 0, 4);
        return BitConverter.ToInt32(b, 0);
    }


    public Int64 getInt64(){
		byte[] b = new byte[8];
		stream.Read (b,0,8);
		return BitConverter.ToInt64(b, 0);
	}

	public int getBigEndianInt() {
		byte[] b = new byte[4];
		stream.Read (b,0,4);
		Array.Reverse (b);
		return BitConverter.ToInt32(b, 0);
	}

	public byte getByte() {
		byte[] b = new byte[1];
		stream.Read(b,0,1);
		return b [0];
	}

	public string getString( string encoding = "utf-8" ) {
		int len = getInt();
		byte[] b = new byte[len];
		stream.Read ( b, 0, len );
		return System.Text.Encoding.UTF8.GetString (b);
	}


	public byte[] getBytes(int count) {
		byte[] b = new byte[ count ];
		stream.Read( b, 0, count );
		return b;
	}
		

	public byte[] getBytes( int p, int count ) {
		byte[] b = new byte[count];
		stream.Read( b, p, count );
		return b;
	}



	public byte[] getArray(){
		return stream.ToArray ();
	}

	public int length {
		get { return unchecked((int)stream.Length); }
	}

	public int toEnd
	{
		get { return unchecked((int)stream.Length - position); }
	}

	public int position
	{
		get { return unchecked((int)stream.Position); }
		set { setPosition(value); }
	}



	public void setPosition(int pos){
		stream.Position = pos;
	}


}


