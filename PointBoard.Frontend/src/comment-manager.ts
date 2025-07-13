import $ from 'jquery';
import { Comment, Point } from './models';
import { createComment, updateComment, deleteComment, getCommentsByPointId } from './api';

export class CommentManager {
  private point: Point | null = null;
  private editingCommentId: string | null = null;

  constructor() {
    $('#closeModal').on('click', this.hideModal.bind(this));
    $('#commentForm').on('submit', this.submitComment.bind(this));
  }

  public show(point: Point) {
    this.point = point;
    this.editingCommentId = null;
    $('#commentText').val('');
    $('#commentColor').val('#cccccc');
    this.renderComments();
    $('#overlay').show();
    $('#commentModal').show();
  }

  private hideModal() {
    $('#overlay').hide();
    $('#commentModal').hide();
    this.point = null;
    this.editingCommentId = null;
  }

  private async renderComments() {
    const container = $('#commentList');
    container.empty();

    if (!this.point) return;

    const pointComments = await getCommentsByPointId(this.point.id)

    pointComments.forEach(comment => {
      const commentDiv = $(`
        <div class="comment-item">
          <span>${comment.text}</span>
          <div class="comment-actions">
            <button class="edit-btn">✏️</button>
            <button class="delete-btn">🗑</button>
          </div>
        </div>
      `).css('border-left', `5px solid ${comment.backgroundColor}`);

      commentDiv.find('.edit-btn').on('click', () => {
        this.editingCommentId = comment.id;
        $('#commentText').val(comment.text);
        $('#commentColor').val(comment.backgroundColor);
      });

      commentDiv.find('.delete-btn').on('click', async () => {
        if (confirm('Удалить комментарий?')) {
          await deleteComment(comment.id);
          $(document).trigger('refresh-points');
        }
      });

      container.append(commentDiv);
    });
  }

  private async submitComment(e: Event) {
    e.preventDefault();
    if (!this.point) return;

    const text = $('#commentText').val();
    const color = $('#commentColor').val();

    const commentText = typeof text === 'string' ? text.trim() : '';
    const backgroundColor = typeof color === 'string' ? color : '#cccccc';

    if (!commentText) return alert('Комментарий не может быть пустым');

    const baseComment = {
      text: commentText,
      backgroundColor: backgroundColor,
      pointId: this.point.id
    };

    try {
      if (this.editingCommentId) {
        await updateComment({ id: this.editingCommentId, ...baseComment });
      } else {
        await createComment(baseComment);
      }
      $(document).trigger('refresh-points');
      this.hideModal();
    } catch (err) {
      console.error(err);
      alert('Ошибка при сохранении комментария');
    }
  }
}
