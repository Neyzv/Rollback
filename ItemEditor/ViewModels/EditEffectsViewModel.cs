using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ItemEditor.Commands;
using Rollback.Common.ORM;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Types;

namespace ItemEditor.ViewModels
{
    internal abstract class EditEffectsViewModel<TRecord, TEffect> : ViewModelBase<EditItemWindow>
        where TRecord : class
        where TEffect : EffectBase, new()
    {
        private const string _defaultSelectedEffectValue = "None";

        private readonly DatabaseAccessor _accessor;
        private readonly EffectId[] _effects;

        private Dictionary<EffectId, EffectBase>? _recordEffects;

        protected TRecord? _record;

        protected abstract string FindRecordById { get; }

        public ICommand CloseCommand =>
            new RelayCommand(_ => View.Close());

        private RelayCommand? _minCommand;
        public ICommand MinCommand =>
            _minCommand ??= new RelayCommand(_ =>
                View.WindowState = WindowState.Minimized);

        private RelayCommand? _searchCommand;
        public ICommand SearchCommand =>
            _searchCommand ??= new RelayCommand(_ => SearchItem());

        private RelayCommand? _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new RelayCommand(_ => SaveItem());

        public EditEffectsViewModel(DatabaseAccessor accessor)
            : base()
        {
            _accessor = accessor;
            _effects = Enum.GetValues<EffectId>();
            LoadEffects();
        }

        protected abstract IEnumerable<EffectBase> GetRecordEffects();

        private bool SavePrevious()
        {
            var res = false;

            if (View.SelectedEffect.Tag is not null)
            {
                if (View.SelectedEffect.Tag is EffectId effectId && _recordEffects!.ContainsKey(effectId))
                {
                    if (_recordEffects[effectId] is EffectInteger integer)
                        if (short.TryParse(View.Value.Text, out var value))
                        {
                            integer.Value = value;

                            res = true;
                        }
                        else
                            new PopUpViewModel("Please enter numeric values...").View.ShowDialog();

                    if (_recordEffects[effectId] is EffectDice dice)
                        if (short.TryParse(View.DiceNum.Text, out var diceNum) && short.TryParse(View.DiceFace.Text, out var diceFace))
                        {
                            dice.DiceNum = diceNum;
                            dice.DiceFace = diceFace;

                            res = true;
                        }
                        else
                            new PopUpViewModel("Please enter numeric values...")
                                .View.ShowDialog();
                }
                else
                    new PopUpViewModel("An error is occured, please restart the application...")
                        .View.ShowDialog();
            }
            else
                res = true;

            return res;
        }

        private void LoadItemRecord(TRecord record)
        {
            _record = record;
            _recordEffects = GetRecordEffects()
                .Where(x => x is not null)
                .ToDictionary(x => x.Id, x => x);

            // Update checkboxes
            foreach (var child in View.Effects.Children)
            {
                if (child is CheckBox checkBox && checkBox.Tag is EffectId effectId)
                    checkBox.IsChecked = _recordEffects.ContainsKey(effectId);
            }
        }

        private void ChargeEffectDatas(EffectBase effect)
        {
            if (effect is EffectInteger integer)
                View.Value.Text = integer.Value.ToString();

            if (effect is EffectDice dice)
            {
                View.DiceNum.Text = dice.DiceNum.ToString();
                View.DiceFace.Text = dice.DiceFace.ToString();
            }

            View.SelectedEffect.Text = effect.Id.ToString();
            View.SelectedEffect.Tag = effect.Id;
        }

        private void ResetEffectValues()
        {
            View.DiceNum.Text = string.Empty;
            View.DiceFace.Text = string.Empty;
            View.Value.Text = string.Empty;
            View.SelectedEffect.Text = _defaultSelectedEffectValue;
            View.SelectedEffect.Tag = default;
        }

        private void CheckBoxEffectClicked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                checkBox.IsChecked = !checkBox.IsChecked;

                if (_record is not null && checkBox.IsChecked == true && SavePrevious() && checkBox.Tag is EffectId effectId &&
                    _recordEffects!.ContainsKey(effectId))
                    ChargeEffectDatas(_recordEffects[effectId]);
            }
        }

        private void CheckBoxEffectDoubleClicked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && _record is not null && SavePrevious())
            {
                checkBox.IsChecked = !checkBox.IsChecked;

                if (checkBox.Tag is EffectId effectId)
                {
                    if (checkBox.IsChecked == true)
                    {
                        var dice = new TEffect()
                        {
                            Id = effectId,
                            Shape = SpellShape.empty,
                        };

                        _recordEffects![effectId] = dice;
                    }
                    else
                    {
                        _recordEffects!.Remove(effectId, out _);
                        ResetEffectValues();
                    }
                }
            }
        }

        private void LoadEffects()
        {
            var effectType = typeof(TEffect);
            var effectIntegerType = typeof(EffectInteger);

            View.Value.IsEnabled = effectType == effectIntegerType || effectType.IsSubclassOf(effectIntegerType);

            var diceVisible = effectType == typeof(EffectDice);
            View.DiceNum.IsEnabled = diceVisible;
            View.DiceFace.IsEnabled = diceVisible;

            ResetEffectValues();
            foreach (var effect in _effects)
            {
                var checkBox = new CheckBox()
                {
                    Content = new Label()
                    {
                        Content = effect.ToString(),
                        Foreground = Brushes.White,
                    },
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Tag = effect,
                };

                checkBox.Click += CheckBoxEffectClicked;
                checkBox.MouseDoubleClick += CheckBoxEffectDoubleClicked;

                View.Effects.Children.Add(checkBox);
            }
        }

        private void SearchItem()
        {
            if (int.TryParse(View.ItemId.Text, out var itemId) &&
                _accessor.SelectSingle<TRecord>(string.Format(FindRecordById, itemId)) is { } itemRecord)
            {
                ResetEffectValues();
                LoadItemRecord(itemRecord);
            }
            else
                new PopUpViewModel("Please enter a valid item id...").View.ShowDialog();
        }

        protected abstract void AssignBinaryDatas(byte[] effectsBin);

        private void SaveItem()
        {
            if (_record is not null && _recordEffects is not null)
            {
                if (SavePrevious())
                {
                    AssignBinaryDatas(EffectManager.SerializeEffects(_recordEffects.Values.ToArray()));
                    _accessor.Update(_record);

                    new PopUpViewModel("Item saved !").View.ShowDialog();
                }
            }
        }
    }
}
