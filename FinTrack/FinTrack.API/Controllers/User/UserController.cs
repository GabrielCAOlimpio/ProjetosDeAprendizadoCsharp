

namespace FinTrack.API.Controllers.User
{
    using FinTrack.API.DTOs.User;
    using FinTrack.API.Interfaces.User;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public record UserNameDTO(string NewName);
        public record UserEmailDTO(string NewEmail);
        public record UserPasswordDTO(string NewPassword);

        [HttpGet("email/{email}")]
        //A rota vai ficar api/user/email/{email}
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDTO user)
        {
            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
        }


        [HttpPatch("{id}/name")]
        public async Task<IActionResult> UpdateUserName([FromRoute] Guid id, [FromBody] UserNameDTO dto)
        {
            await _userService.UpdateUserNameAsync(id, dto.NewName);
            return NoContent();
        }

        [HttpPatch("{id}/email")]
        public async Task<IActionResult> UpdateUserEmail([FromRoute] Guid id, [FromBody] UserEmailDTO dto)
        {
            await _userService.UpdateUserEmailAsync(id, dto.NewEmail);
            return NoContent();
        }
        [HttpPatch("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword([FromRoute] Guid id, [FromBody] UserPasswordDTO dto)
        {
            await _userService.UpdateUserPasswordAsync(id, dto.NewPassword);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/recover")]
        public async Task<IActionResult> RecoverUser([FromRoute] Guid id)
        {
            await _userService.RecoverUserAsync(id);
            return NoContent();
        }

    }

    
}