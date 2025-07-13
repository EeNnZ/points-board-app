import Konva from 'konva';
import $ from 'jquery';
import { getPoints, createPoint, deletePoint, getCommentsByPointId, updatePoint } from './api';
import { Point, Comment } from './models';
import { CommentManager } from './comment-manager';
import { PointEditManager } from './point-edit-manager';
import { Group } from 'konva/lib/Group';

export class PointManager {
  private isUpdating = false;
  private stage: Konva.Stage;
  private layer: Konva.Layer;
  private commentManager: CommentManager;
  private pointEditManager: PointEditManager;

  constructor() {
    const container = document.getElementById("container")!;
    const width = container.clientWidth;
    const height = container.clientHeight;

    this.stage = new Konva.Stage({
      container: "container",
      width, height
    })

    this.layer = new Konva.Layer();
    this.stage.add(this.layer);

    this.commentManager = new CommentManager();
    this.pointEditManager = new PointEditManager();

    this.bindEvents();
    this.loadAndRenderPoints();

    window.addEventListener("resize", () => this.resizeStage());
  }

  private resizeStage() {
    const container = document.getElementById('container')!;
    const width = container.clientWidth;
    const height = container.clientHeight;

    this.stage.size({ width, height });
    this.stage.draw();
  }

  private bindEvents() {
    $('#add-point-btn').on('click', async () => {
      if (this.isUpdating) return;
      this.isUpdating = true;
      const newPoint = {
        x: Math.random() * 600 + 50,
        y: Math.random() * 400 + 50,
        radius: 20,
        color: '#00aaff'
      };
      try {
        await createPoint(newPoint);
        await this.loadAndRenderPoints();
      } catch (err) {
        console.error('Ошибка при добавлении точки:', err);
      } finally {
        this.isUpdating = false;
      }
    });

    $('#edit-point-btn').on('click', () => {
      this.pointEditManager.show();
    });

    $(document).on('refresh-points', async () => {
      if (this.isUpdating) return;
      this.isUpdating = true;
      try {
        await this.loadAndRenderPoints();
      } finally {
        this.isUpdating = false;
      }
    });
  }

  private async loadAndRenderPoints() {
    this.layer.destroyChildren();

    try {
      const points = await getPoints();
      points.forEach(p => this.drawPoint(p));
      this.layer.draw();
    } catch (error) {
      console.error('Ошибка загрузки точек:', error);
    }
  }

  private drawPoint(p: Point) {
    const group = new Konva.Group({
      x: p.x,
      y: p.y,
      draggable: true,
      id: `point-${p.id}`
    });

    const circle = new Konva.Circle({
      radius: p.radius,
      fill: p.color,
      stroke: 'black',
      strokeWidth: 1
    });

    group.add(circle);

    let commentTooltips: Konva.Label[] = [];

    group.on('mouseenter', async () => {
      try {
        commentTooltips.forEach(t => t.destroy());
        commentTooltips = [];
        const comments: Comment[] = await getCommentsByPointId(p.id);
        if (comments.length > 0) {
          let yOffset = p.radius + 8;
          comments.forEach(comment => {
            const label = new Konva.Label({
              x: 0,
              y: yOffset,
              visible: true
            });
            const tag = new Konva.Tag({
              fill: comment.backgroundColor || '#fff',
              stroke: '#0f375e',
              strokeWidth: 1.5,
              cornerRadius: 5,
              shadowColor: '#0f375e',
              shadowBlur: 2,
              shadowOffset: { x: 0, y: 1 },
              shadowOpacity: 0.08
            });
            const text = new Konva.Text({
              text: comment.text,
              fontSize: 14,
              fill: 'black',
              padding: 6,
            });
            label.add(tag);
            label.add(text);
            
            label.x(-label.width() / 2);
            group.add(label);
            commentTooltips.push(label);
            yOffset += text.height() + 3;
          });
          this.layer.draw();
        }
      } catch (error) {
        console.error(`Ошибка загрузки комментариев для точки ${p.id}:`, error);
      }
    });

    group.on('mouseleave', () => {
      commentTooltips.forEach(t => t.destroy());
      commentTooltips = [];
      this.layer.draw();
    });

    group.on('contextmenu', (e) => {
      e.evt.preventDefault();
      this.commentManager.show(p);
    });

    group.on('dblclick', async () => {
      if (this.isUpdating) return;
      if (confirm('Удалить эту точку?')) {
        this.isUpdating = true;
        try {
          await deletePoint(p.id);
          await this.loadAndRenderPoints();
        } catch (error) {
          console.error('Ошибка при удалении точки:', error);
        } finally {
          this.isUpdating = false;
        }
      }
    });

    group.on("dragend", async () => {
      await this.updatePointPositions(group, p);
    })

    this.layer.add(group);
  }

  private async updatePointPositions(group: Group, p: Point) {
    this.isUpdating = true;
    const position = group.position();
    try {
      await updatePoint(p.id, {
        x: position.x,
        y: position.y,
        radius: p.radius,
        color: p.color
      });
    } catch (error) {
      console.error('Ошибка при обновлении позиции точки:', error);
    } finally {
      this.isUpdating = false;
    }
  }
}