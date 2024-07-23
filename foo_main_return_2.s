	.text
	.globl	foo
foo:
	pushq	%rbp
	movq	%rsp, %rbp
	movl	$2, %eax
	popq	%rbp
	ret
	.globl	main
main:
	pushq	%rbp
	movq	%rsp, %rbp
	subq	$32, %rsp
	call	__main
	call	foo
	movl	$2, %eax
	addq	$32, %rsp
	popq	%rbp
	ret
