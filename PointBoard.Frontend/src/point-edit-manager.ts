import $ from 'jquery';
import { getPoints, updatePoint } from './api';
import { Point } from './models';

export class PointEditManager {
  private points: Point[] = [];

  constructor() {
    $('#closeEditPointModal').on('click', this.hideModal.bind(this));
    $('#editPointForm').on('submit', this.submitEdit.bind(this));
    $('#editPointSelect').on('change', this.updateFields.bind(this));
  }

  public async show() {
    this.points = await getPoints();
    const select = $('#editPointSelect');
    select.empty();
    this.points.forEach(p => {
      const opt = $(`<option></option>`) 
        .val(p.id)
        .text(`#${p.id.slice(0,6)} (${Math.round(p.x)}, ${Math.round(p.y)})`)
        .attr('data-color', p.color)
        .attr('data-radius', p.radius);
      select.append(opt);
    });
    this.updateFields();
    $('#editPointOverlay').show();
    $('#editPointModal').show();
  }

  private hideModal() {
    $('#editPointOverlay').hide();
    $('#editPointModal').hide();
  }

  private updateFields() {
    const select = $('#editPointSelect');
    const opt = select.find('option:selected');
    if (opt.length) {
      $('#editPointColor').val(opt.attr('data-color') || '');
      $('#editPointRadius').val(opt.attr('data-radius') || '');
    }
  }

  private async submitEdit(e: JQuery.SubmitEvent) {
    e.preventDefault();
    const select = $('#editPointSelect');
    const id = select.val() as string;
    const color = $('#editPointColor').val() as string;
    const radius = parseFloat($('#editPointRadius').val() as string);
    const point = this.points.find(p => p.id === id);
    if (!point || !color || isNaN(radius)) return;
    try {
      await updatePoint(id, { x: point.x, y: point.y, radius, color });
      $(document).trigger('refresh-points');
      this.hideModal();
    } catch (err) {
      alert('Ошибка при обновлении точки');
    }
  }
}
