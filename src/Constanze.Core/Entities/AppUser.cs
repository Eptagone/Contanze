// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Constanze.Core.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class AppUser
{
	/// <summary>
	/// Initialize a new instance of <see cref="AppUser"/>.
	/// </summary>
	public AppUser()
	{
		this.Conversations = new HashSet<Conversation>();
	}

	/// <summary>
	/// The user's unique identifier in the database.
	/// </summary>
	[Key]
	public int Key { get; set; }

	/// <summary>
	/// The user's unique identifier in Telegram.
	/// </summary>
	[Required]
	public long Id { get; set; }

	/// <summary>
	/// The user's first name.
	/// </summary>
	[Required]
	public string FirstName { get; set; } = null!;

	/// <summary>
	/// Optional. The user's last name.
	/// </summary>
	public string? LastName { get; set; }

	/// <summary>
	/// Optional. The user's username.
	/// </summary>
	public string? Username { get; set; }

	/// <summary>
	/// Optional. The user's language code.
	/// </summary>
	public string? LanguageCode { get; set; }

	/// <summary>
	/// List of conversations of the user with the bot.
	/// </summary>
	[InverseProperty("User")]
	public ICollection<Conversation> Conversations { get; }
}
