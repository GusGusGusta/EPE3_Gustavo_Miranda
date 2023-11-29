// Importación de namespaces necesarios
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


[Route("Medico")]
[ApiController]
public class MedicoController : ControllerBase
{
   // Cadena a la base de datos
    private readonly string _connectionString;

    
    public MedicoController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MySqlConnection");
    }

    // Metodo get
    [HttpGet]
    public async Task<IActionResult> ListarMedicos()
    {
        try
        {
              // conexion a las librerias 
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // se genera copnsulta
        
                string query = "SELECT * FROM Medico";

                // listamos a los medicos

                List<Medico> medicos = new List<Medico>();

               
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (var Lector = await command.ExecuteReaderAsync())
                {
                    
                    while (await Lector.ReadAsync())
                    {
                        // se crea el metodo medico
                        medicos.Add(new Medico
                        {
                            idMedico = Lector.GetInt32(0),
                            NombreMed = Lector.GetString(1),
                            ApellidoMed = Lector.GetString(2),
                            RunMed = Lector.GetString(3),
                            Eunacom = Lector.GetString(4),
                            NacionalidadMed = Lector.GetString(5),
                            Especialidad = Lector.GetString(6),
                            Horario = Lector.GetString(7),
                            TarifaHr = Lector.GetInt32(8)
                        });
                    }
                }

                // respuesta 200
                return StatusCode(200, medicos);
            }
        }
        catch (Exception ex)
        {
            // error 500
            return StatusCode(500, "No fue posible listar: " + ex);
        }
    }

    // consulta por id
    [HttpGet("{idMedico}")]
    public async Task<IActionResult> ObtenerMedico(int id)
    {
        try
        {
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta SQL para medico por id
                string Consulta = "SELECT * FROM Medico WHERE idMedico = @id";

                
                Medico medico = new Medico();

                
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                   
                    using (var Lector = await command.ExecuteReaderAsync())
                    {
                       // validador de dato
                        if (await Lector.ReadAsync())
                        {
                           
                            medico.idMedico = Lector.GetInt32(0);
                            medico.NombreMed = Lector.GetString(1);
                            medico.ApellidoMed = Lector.GetString(2);
                            medico.RunMed = Lector.GetString(3);
                            medico.Eunacom = Lector.GetString(4);
                            medico.NacionalidadMed = Lector.GetString(5);
                            medico.Especialidad = Lector.GetString(6);
                            medico.Horario = Lector.GetString(7);
                            medico.TarifaHr = Lector.GetInt32(8);

                            //respuesta 200
                            return StatusCode(200, medico);
                        }
                        else
                        {
                            //error 400

                            return StatusCode(404, "No se encontro");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // erro 500
            return StatusCode(500, "No se puede realizar: " + ex);
        }
    }

    // Metodo post
    [HttpPost]
    public async Task<IActionResult> Nuevomedico([FromBody] Medico medico)
    {
        try
        {
            // Uso de "using" para garantizar la liberación de recursos
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // consulta mediante sql
                string Consulta = "INSERT INTO Medico (NombreMed, ApellidoMed, RunMed, Eunacom, NacionalidadMed, Especialidad, horario, TarifaHr) VALUES (@NombreMed, @ApellidoMed, @RunMed, @Eunacom, @NacionalidadMed, @Especialidad, @horario, @TarifaHr)";

                
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    
                    command.Parameters.AddWithValue("@NombreMed", medico.NombreMed);
                    command.Parameters.AddWithValue("@ApellidoMed", medico.ApellidoMed);
                    command.Parameters.AddWithValue("@RunMed", medico.RunMed);
                    command.Parameters.AddWithValue("@Eunacom", medico.Eunacom);
                    command.Parameters.AddWithValue("@NacionalidadMed", medico.NacionalidadMed);
                    command.Parameters.AddWithValue("@Especialidad", medico.Especialidad);
                    command.Parameters.AddWithValue("@horario", medico.Horario);
                    command.Parameters.AddWithValue("@TarifaHr", medico.TarifaHr);

                 
                    await command.ExecuteNonQueryAsync();

                    // respuesta codigo 201
                    return StatusCode(201, $"Medico creado correctamente: {medico}");
                }
            }
        }
        catch (Exception ex)
        {
            // error 500
            return StatusCode(500, "No se pudo guardar  " + ex);
        }
    }

    // metodo put
    [HttpPut("{id}")]
    public async Task<IActionResult> ModificarMedico(int id, [FromBody] Medico medico)
    {
        try
        {
            
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta Query
                string consulta = "UPDATE Medico SET NombreMed = @NombreMed, ApellidoMed = @ApellidoMed, RunMed = @RunMed, Eunacom = @Eunacom, NacionalidadMed = @NacionalidadMed, Especialidad = @Especialidad, horario = @horario, TarifaHr = @TarifaHr WHERE idMedico = @id";

               
                using (MySqlCommand command = new MySqlCommand(consulta, connection))
                {
                   
                    command.Parameters.AddWithValue("@NombreMed", medico.NombreMed);
                    command.Parameters.AddWithValue("@ApellidoMed", medico.ApellidoMed);
                    command.Parameters.AddWithValue("@RunMed", medico.RunMed);
                    command.Parameters.AddWithValue("@Eunacom", medico.Eunacom);
                    command.Parameters.AddWithValue("@NacionalidadMed", medico.NacionalidadMed);
                    command.Parameters.AddWithValue("@Especialidad", medico.Especialidad);
                    command.Parameters.AddWithValue("@horario", medico.Horario);
                    command.Parameters.AddWithValue("@TarifaHr", medico.TarifaHr);
                    command.Parameters.AddWithValue("@id", id);

                   


                    await command.ExecuteNonQueryAsync();


                    // retorno respuesta 200
                    return StatusCode(200, "Registro editado con exito");
                }
            }
        }

        catch (Exception ex)

        {

            //retorno error 500
            return StatusCode(500, "No se pudo actualizar y editar al medico : " + ex);

        }
    }

    // metodo delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarMedico(int id)

    {
        try
        {
           
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Consulta SQL para eliminar
                string Consulta = "DELETE FROM Medico WHERE idMedico = @id";

                /
                using (MySqlCommand command = new MySqlCommand(Consulta, connection))
                {
                    
                    command.Parameters.AddWithValue("@id", id);

                   
                    var ELim = await command.ExecuteNonQueryAsync();

                   
                    if (ELim == 0)
                    {
                        // error 404
                        return StatusCode(404, "Registro no encontrado con exito");
                    }

                    else
                    {
                        
                        return StatusCode(200, $"Medico con el ID {id} eliminado correctamente");
                    }
                }
            }
        }

        catch (Exception ex)
        {
            // error 500
            return StatusCode(500, "No se pudo eliminar : " + ex);

        }
    }
}