using DesafioBibliotecaApi.Enumerados;
using System;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.DTOs
{
    public class ResultDTO
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
        public Object Result { get; set; }

        public static ResultDTO ErroResult(string errorMessage)
        {
            return new ResultDTO
            {
                Success = false,
                Errors = new string[] { errorMessage },
                Result = null
            };
        }
        public static ResultDTO SuccessResult(object result = null)
        {
            return new ResultDTO
            {
                Success = true,
                Errors = null,
                Result = result
            };
        }

    }
    
}
