import { Point, Comment } from './models';
import $ from 'jquery';

const API = '/api';

/** Получить все точки */
export async function getPoints(): Promise<Point[]> {
  const points = $.get(`${API}/points`);
  return points;
}

/** Создать новую точку */
export async function createPoint(point: Omit<Point, 'id' | 'comments'>): Promise<void> {
  await $.ajax({
    url: `${API}/points`,
    method: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(point)
  });
}

/** Удалить точку по ID */
export async function deletePoint(id: string): Promise<void> {
  await $.ajax({
    url: `${API}/points/${id}`,
    method: 'DELETE'
  });
}

/** Обновить точку по ID */
export async function updatePoint(id: string, data: Omit<Point, 'id' | 'comments'>): Promise<void> {
  await $.ajax({
    url: `${API}/points/${id}`,
    method: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(data)
  });
}

/** Получить комментарии по ID точки */
export async function getCommentsByPointId(pointId: string): Promise<Comment[]> {
  return $.get(`/api/points/${pointId}/comments`);
}



/** Создать комментарий */
export async function createComment(comment: Omit<Comment, 'id'>): Promise<void> {
  await $.ajax({
    url: `${API}/comments`,
    method: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(comment)
  });
}

/** Обновить комментарий по ID */
export async function updateComment(comment: Comment): Promise<void> {
  await $.ajax({
    url: `${API}/comments/${comment.id}`,
    method: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(comment)
  });
}

/** Удалить комментарий по ID */
export async function deleteComment(id: string): Promise<void> {
  await $.ajax({
    url: `${API}/comments/${id}`,
    method: 'DELETE'
  });
}
