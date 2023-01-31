using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using wakeApp.Data;
using wakeApp.Dtos;
using wakeApp.Repositories;

namespace wakeApp.Controllers
{
    public class CommentDtoesController : Controller
    {
        private readonly wakeAppContext _context;
        private readonly ICommentRepository _commentRepository;

        public CommentDtoesController(wakeAppContext context, ICommentRepository commentRepository)
        {
            _context = context;
            _commentRepository = commentRepository;
        }

        // GET: CommentDtoes
        public async Task<IActionResult> Index()
        {
              return View(await _context.CommentDto.ToListAsync());
        }

        // GET: CommentDtoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CommentDto == null)
            {
                return NotFound();
            }

            var commentDto = await _context.CommentDto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentDto == null)
            {
                return NotFound();
            }

            return View(commentDto);
        }

        // POST: CommentDtoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommentText,UserId,ChannelId,PostId")] CommentDto commentDto)
        {
            commentDto.Flag = true;
            var id = commentDto.PostId;
            commentDto = _commentRepository.AddComment(commentDto);

            if (commentDto == null)
            {
                RedirectToAction(nameof(Index));
            }
            return Redirect("/PostVideos/Details/" + id);
        }

        // GET: CommentDtoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CommentDto == null)
            {
                return NotFound();
            }

            var commentDto = await _context.CommentDto.FindAsync(id);
            if (commentDto == null)
            {
                return NotFound();
            }
            return View(commentDto);
        }

        // POST: CommentDtoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentText,UserId,ChannelId,PostId,Flag")] CommentDto commentDto)
        {
            if (id != commentDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentDtoExists(commentDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(commentDto);
        }

        // GET: CommentDtoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CommentDto == null)
            {
                return NotFound();
            }

            var commentDto = await _context.CommentDto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentDto == null)
            {
                return NotFound();
            }

            return View(commentDto);
        }

        // POST: CommentDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CommentDto == null)
            {
                return Problem("Entity set 'wakeAppContext.CommentDto'  is null.");
            }
            var commentDto = await _context.CommentDto.FindAsync(id);
            if (commentDto != null)
            {
                _context.CommentDto.Remove(commentDto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentDtoExists(int id)
        {
          return _context.CommentDto.Any(e => e.Id == id);
        }
    }
}
