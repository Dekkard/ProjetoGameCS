public class ForcedExitException : Exception {

    private bool hasMessange;
    public bool HasMessage {get => hasMessange;}
    public ForcedExitException(){
        hasMessange = false;
    }
    public ForcedExitException(String? msg) : base(msg){
        hasMessange = true;
    }
}