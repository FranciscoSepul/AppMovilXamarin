namespace PDRProvBackEnd.DTOModels
{

public class ResponseModel
{
    public int Code {get; set;}
    public string Message {get; set;}

        public static ResponseModel ErrorInterno()
        {
            var res = new ResponseModel()
            {
                Code = 9,
                Message = "Error interno."
            };
            return res;
        }

}
}