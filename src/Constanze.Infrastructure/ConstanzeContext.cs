// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Constanze.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Constanze.Infrastructure;
public class ConstanzeContext : DbContext
{
	/// <summary>
	/// Initializes a new instance of <see cref="ConstanzeContext"/>.
	/// </summary>
	/// <param name="options">The options for this context.</param>
	public ConstanzeContext(DbContextOptions<ConstanzeContext> options) : base(options)
	{
	}

	/// <summary>
	/// List of users.
	/// </summary>
	public DbSet<AppUser> Users { get; set; }

	/// <summary>
	/// List of conversations.
	/// </summary>
	public DbSet<Conversation> Conversations { get; set; }

	/// <summary>
	/// List of dialog messages.
	/// </summary>
	public DbSet<DialogMessage> DialogMessages { get; set; }
}
