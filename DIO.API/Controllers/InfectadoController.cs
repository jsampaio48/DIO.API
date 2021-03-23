using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIO.API.Collections;
using DIO.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DIO.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public Data.MongoDB MongoDB { get => _mongoDB; set => _mongoDB = value; }

        public InfectadoController(Data.MongoDB mongoDB)
        {
            MongoDB = mongoDB;
            _infectadosCollection = MongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] infectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectados([FromBody] infectadoDto dto)
        {

            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(i => i.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo));

            return Ok("Atualizado com sucesso!");
        }

        [HttpDelete]
        public ActionResult DeletarInfectados(DateTime dataNascimento)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(i => i.DataNascimento == Convert.ToDateTime(dataNascimento)));

            return Ok("Deletado com sucesso!");
        }

    }

}
