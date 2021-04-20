using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

public class Server {

    void main() {
        //JsonSerializer e;
        //DataContractJsonSerializer ser = new DataContractJsonSerializer() ;


    }

}

[DataContract]
public class Bobo {

    [DataMember]
    private string name;

    [DataMember]
    private int age;

}
