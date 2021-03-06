﻿using System;
using System.Collections.Generic;

namespace apartmenthostService.DataObjects
{
    public class CardRequestDTO
    {
        // Лмимит
        public int Limit { get; set; }
        // Пропуск
        public int Skip { get; set; }
        // Уникальный идентификатор Карточки
        public string Id { get; set; }
        // Наименование Карточки
        public string Name { get; set; }
        // Адрес Жилья
        public string Adress { get; set; }
        // Тип адреса
        public List<string> AdressTypes { get; set; }
        // Уникальный идентификатор Google Places
        public string PlaceId { get; set; }
        // Левый верхний угол области Широта
        public double? SwLat { get; set; }
        // Левый верхний угол области Долгота
        public double? SwLong { get; set; }
        // Правый нижний угол области Широта
        public double? NeLat { get; set; }
        // Правый нижний угол области Долгота
        public double? NeLong { get; set; }
        // Уникальный Идентификатор Владельца
        public string UserId { get; set; }
        // Описание Карточки
        public string Description { get; set; }
        // Уникальный Идентификатор Жилья
        public string ApartmentId { get; set; }
        // Тип Жилья
        public List<string> Type { get; set; }
        // Дополнительные опции Жилья
        public string Options { get; set; }
        // Дата доступности с
        public DateTime? AvailableDateFrom { get; set; }
        // Дата доступности по
        public DateTime? AvailableDateTo { get; set; }
        // Цена за день с
        public decimal? PriceDayFrom { get; set; }
        // Цена за день по
        public decimal? PriceDayTo { get; set; }
        // Цена за период с
        public decimal? PricePeriodFrom { get; set; }
        // Цена за период по
        public decimal? PricePeriodTo { get; set; }
        // Тип проживания
        public List<string> Cohabitation { get; set; }
        // Пол проживающего
        public List<string> ResidentGender { get; set; }
        // Пол проживающего
        public List<string> Genders { get; set; }
        // Избранное (Уникальный идентификатор пользователя)
        public string IsFavoritedUserId { get; set; }
        // Дата добавления с
        public DateTime? CreatedAtFrom { get; set; }
        // Дата добавления по
        public DateTime? CreatedAtTo { get; set; }
    }
}