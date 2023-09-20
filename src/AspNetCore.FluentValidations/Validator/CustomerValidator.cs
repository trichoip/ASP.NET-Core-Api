using AspNetCore.FluentValidations.Models;
using FluentValidation;

namespace AspNetCore.FluentValidations.Validator
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            // lưu ý: nếu lấy object.properties mà object = null thì sẽ bị lỗi
            // nên phải check null bằng cách dùn .When(object => object != null)
            // hoặc When(c => c.Address != null, () =>{}) | xem address là hiểu

            // áp dụng CascadeMode.Stop cho tất cả các rule ở class này
            RuleLevelCascadeMode = CascadeMode.Stop;

            #region Id
            RuleFor(c => c.Id)
                   .NotNull().WithMessage("Id must not null")
                   .LessThan(10)
                   .GreaterThanOrEqualTo(0)
                   // đổi tên mặc định khi báo lỗi , lưu ý là không set WithMessage
                   .WithName(c => "properties: " + nameof(c.Id));

            #endregion

            #region Forename
            RuleFor(c => c.Forename)
                    .NotNull().WithMessage(c => $" NotNull - Forename: {c.Forename} - Discount: {c.Discount}")
                    .NotEmpty().WithMessage(" NotEmpty - PropertyName: {PropertyName}")
                    .NotEqual("foo", StringComparer.OrdinalIgnoreCase).WithMessage("NotEqual - ComparisonValue: {ComparisonValue} - ComparisonProperty: {ComparisonProperty}")
                    .Length(1, 20).WithMessage("Length - MinLength: {MinLength} - MaxLength: {MaxLength} - TotalLength: {TotalLength} - PropertyName: {PropertyName} - PropertyValue: {PropertyValue}");

            #endregion

            #region Surname
            RuleFor(c => c.Surname)
                   // nếu không có CascadeMode.Stop thì khi gặp lỗi ở NotNull thì nó vẫn check các rule ở dưới nó
                   // còn nếu có CascadeMode.Stop thì khi gặp lỗi ở đâu thì nó dừng lại và không check các rule ở dưới nó
                   // mặc định là CascadeMode.Continue
                   .Cascade(CascadeMode.Stop)
                   .NotNull().WithMessage("NotNull {PropertyPath} {PropertyName} {PropertyValue}")
                   .NotEmpty().WithMessage("NotEmpty {PropertyPath} {PropertyName} {PropertyValue}")
                   .Matches("some regex here").WithMessage("{RegularExpression}")
                   .Must(surname => surname == "Foo")
                   .Must((customer, surname) => surname != customer.Forename)
                   .MaximumLength(250)
                   .MinimumLength(10)
                   .Length(1, 250).WithMessage("{MinLength} {MaxLength} {TotalLength}")
                   .Equal(customer => customer.Forename).WithMessage("{ComparisonValue}  {ComparisonProperty}");
            #endregion

            #region Discount

            RuleFor(c => c.Discount)
                   .NotNull()
                   .GreaterThan(0)
                   .GreaterThanOrEqualTo(0)
                   //.GreaterThan(c => c.MinimumCreditLimit)
                   //.GreaterThanOrEqualTo(c => c.MinimumCreditLimit)
                   .LessThan(100)
                   .LessThanOrEqualTo(100)
                   //.LessThan(c => c.MaxCreditLimit)
                   //.LessThanOrEqualTo(c => c.MaxCreditLimit)
                   .InclusiveBetween(0, 10).WithMessage(string.Format("values: {0} - {1}", 0, 10))
                   .ExclusiveBetween(1, 10).WithMessage("{From} {To}")
                   .InclusiveBetween(1, 10).WithMessage("{From} {To}")
                   // Tổng số không được nhiều hơn 4 chữ số, có cho phép 2 số thập phân. 5 chữ số và 3 số thập phân đã được tìm thấy.
                   // 123.4500
                   .PrecisionScale(4, 2, false).WithMessage("{ExpectedPrecision}  {ExpectedScale}  {Digits} {ActualScale}");

            #endregion

            #region Address
            RuleFor(c => c.Address)
               .NotNull().WithMessage("error is required")
               // lưu ý nếu Address == null thì sẽ bị lỗi chổ a.Count() cho nên phải chèck null
               // cách 1: dùng .When(c => c.Address != null)
               .Must(a => a.Count() >= 2).WithMessage("Address must have at least 2 item")
               // nếu when không thêm ApplyConditionTo.CurrentValidator thì nó thỏa mản nó sẽ check hết tất cả các rule ở trên nó
               // nếu When áp dụng ApplyConditionTo.CurrentValidator  thì nó chỉ check 1 rule ở trước nó
               // nếu thỏa mản thì check rule ở trên nó là Must
               .When(c => c.Address != null, ApplyConditionTo.CurrentValidator)
               // ForEach này giống với RuleForEach ở dưới
               // hai cái này đều có thể cộng dồn error với nhau
               .ForEach(rule =>
               {
                   rule.Length(10, 20).WithMessage("between 10 and 20 characters");
               });

            RuleForEach(c => c.Address)
                .Length(10, 20).WithMessage("Address must be between 10 and 20 characters");

            // cách 2: không dùng .When(c => c.Address != null)
            When(c => c.Address != null, () =>
            {
                RuleFor(c => c.Address)
                    .Must(a => a.Count() >= 2).WithMessage("When Address must have at least 2 item");
            }).Otherwise(() =>
            {
                //....
            });

            #endregion

            #region Department
            // có 2 cách tương tự nhau và đều có thể cộng dồn error với nhau
            // ưu tiên cách 1

            // Cách 1: nên dùng 
            RuleFor(c => c.Department)
                .NotNull().WithMessage("Department is required")
                .SetValidator(new DepartmentValidator());

            // cách 2: không nên dùng
            // cách này phải add dụng  .When(c => c.Department != null) vì nếu không thì sẽ bị lỗi nếu Department = null
            RuleFor(c => c.Department.Name)
                .NotNull().WithMessage("Department Name is required")
                .Length(10, 20).WithMessage("Department Name must be between 10 and 20 characters")
                .When(c => c.Department != null);

            RuleFor(c => c.Department.Id)
                .NotNull().WithMessage("Department Id is required")
                .GreaterThan(0).WithMessage("Department Id must be greater than 0")
                .When(c => c.Department != null);
            #endregion

            #region Orders

            RuleFor(x => x.Orders)
              .Must(x => x.Count >= 1).WithMessage("list more than 1") // check list cách 1
              .ChildRules(orders => // check list cách 2
              {
                  orders.RuleFor(x => x).Must(x => x.Count >= 1).WithMessage("list ChildRules more than 1");
                  orders.RuleFor(x => x.Count).GreaterThanOrEqualTo(1).WithMessage("list ChildRules GreaterThanOrEqualTo more than 1");
              })
              // ForEach giống với RuleForEach ở dưới
              .ForEach(orderRule =>
              {
                  // 3 cách orderRule dưới giống nhau 
                  orderRule.SetValidator(new OrderValidator()); // cách 1 orderRule
                  orderRule.Must(order => order.Total > 0).WithMessage("ForEach Total more than 0"); // cách 2 orderRule
                  orderRule.ChildRules(order => // cách 3 orderRule
                  {
                      order.RuleFor(x => x.Total).GreaterThan(0).WithMessage("ChildRules  RuleFor Total");
                  });
              });

            // giống với ForEach ở trên
            RuleForEach(x => x.Orders)
              .SetValidator(new OrderValidator())
              .Must(order => order.Total >= 1).WithMessage("list 2 more than 1")
              .ChildRules(order =>
              {
                  order.RuleFor(x => x.Total).GreaterThan(0).WithMessage("ChildRules  RuleFor Total");
              });

            #endregion

            #region Photo
            //  chỉ định ApplyConditionTo.CurrentValidatornhư một phần của mọi điều kiện
            // Trong ví dụ sau, lệnh gọi đầu tiên When chỉ áp dụng cho lệnh gọi tới Matches,
            // chứ không áp dụng cho lệnh gọi tới NotEmpty.
            // Lệnh gọi thứ hai When chỉ áp dụng cho lệnh gọi tới Empty.

            RuleFor(customer => customer.Photo)
                .NotEmpty()
                .Matches(@"photo")
                .When(customer => customer.IsPreferredCustomer, ApplyConditionTo.CurrentValidator)
                .Empty()
                .When(customer => !customer.IsPreferredCustomer, ApplyConditionTo.CurrentValidator);

            #endregion

            #region Custom

            RuleFor(c => c.Orders)
                .ListMustContainFewerThan2(1)
                .Custom((list, context) =>
                {
                    if (list.Count > 3)
                    {
                        context.AddFailure("b The list must contain 3 items or fewer");
                    }
                });

            RuleFor(c => c.Surname)
                .Must(BeOver18);

            //Include(new CustomerAgeValidator());
            //Include(new CustomerNameValidator());

            #endregion

        }

        protected bool BeOver18(string date)
        {
            return true;
        }
    }

    public static class MyCustomValidators
    {
        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {
            return ruleBuilder.Must(list => list.Count < num).WithMessage("The list contains too many items");
        }

        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan2<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {

            return ruleBuilder.Must((rootObject, list, context) =>
            {
                context.MessageFormatter.AppendArgument("MaxElements", num);
                return list.Count < num;
            })
            .WithMessage("a {PropertyName} must contain fewer than {MaxElements} items.");
        }

        public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan3<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {

            return ruleBuilder.Must((rootObject, list, context) =>
            {
                context.MessageFormatter
                  .AppendArgument("MaxElements", num)
                  .AppendArgument("TotalElements", list.Count);

                return list.Count < num;
            })
            .WithMessage("{PropertyName} must contain fewer than {MaxElements} items. The list contains {TotalElements} element");
        }

        public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainFewerThan4<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
        {

            return ruleBuilder.Custom((list, context) =>
            {
                if (list.Count > 10)
                {
                    context.AddFailure("The list must contain 10 items or fewer");
                }
            });
        }
    }
}
