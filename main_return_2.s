	.text
	.globl	main
main:
	pushq	%rbp
	movq	%rsp, %rbp
	subq	$32, %rsp
	call	__main
	movl	$2, %eax
	addq	$32, %rsp
	popq	%rbp
	ret